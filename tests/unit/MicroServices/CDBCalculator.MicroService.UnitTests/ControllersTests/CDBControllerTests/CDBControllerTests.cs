using Application.CQRS.Requests.Commands;
using Application.CQRS.Responses.CDBCalculator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Reflection.Metadata;
using CdbController = CDBCalculator.Controllers.CdbController;

namespace CDBCalculator.MicroService.UnitTests.ControllersTests.CDBControllerTests;

public class CDBControllerTests
{
    private readonly ILogger<CdbController> _loggerMock = Substitute.For<ILogger<CdbController>>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();

    private CdbController CreateController() =>
        new(_loggerMock, _mediator);

    [Theory]
    [InlineData(200, typeof(OkObjectResult))]
    [InlineData(400, typeof(BadRequestObjectResult))]
    [InlineData(422, typeof(UnprocessableEntityObjectResult))]
    [InlineData(500, typeof(ContentResult))]
    public async Task Simulate_ReturnsExpectedResponse(int statusCode, Type expectedType)
    {
        var response = new SimulateCdbResponse { StatusCode = statusCode, Message = "Mensagem", Success = false };

        _mediator.Send(Arg.Any<SimulateCdbCommand>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(response));

        var result = await CreateController().Simulate(6, 1000.00m);

        Assert.IsType(expectedType, result);

    }

    [Fact]
    public async Task Simulate_WhenExceptionThrown_Returns500ContentResult()
    {
        _mediator.Send(Arg.Any<SimulateCdbCommand>(), Arg.Any<CancellationToken>())
               .ThrowsAsync(new InvalidOperationException("Falhou"));


        var controller = CreateController();
        var result = await controller.Simulate(6, 1000.0m);

        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal(500, contentResult.StatusCode);
        Assert.Equal("Falhou", contentResult.Content);
    }
    
    [Fact]
    public async Task Simulate_WithUnexpectedStatusCode_ReturnsCustomContentResult()
    {
        var response = new SimulateCdbResponse
        {
            StatusCode = 418,
            Message = "Sou um bule de chá",
            Success = false
        };

        _mediator.Send(Arg.Any<SimulateCdbCommand>(), Arg.Any<CancellationToken>())
               .Returns(Task.FromResult(response));


        var controller = CreateController();
        var result = await controller.Simulate(6, 1000.0m);

        var content = Assert.IsType<ContentResult>(result);
        Assert.Equal(418, content.StatusCode);
        Assert.Equal("Sou um bule de chá", content.Content);
    }

    [Fact]
    public async Task Simulate_WhenResponseIsNull_ReturnsEmptyContentResult()
    {
        _mediator.Send(Arg.Any<SimulateCdbCommand>(), Arg.Any<CancellationToken>())
                        .Returns(Task.FromResult<SimulateCdbResponse>(null!));


        var controller = CreateController();
        var result = await controller.Simulate(6, 1000.0m);

        var content = Assert.IsType<ContentResult>(result);
        Assert.NotNull(content.Content); 
    }
    [Fact]
    public async Task Simulate_ShouldPassConvertedInitialValueAndMonthsCorrectly()
    {
        SimulateCdbCommand? capturedCommand = null;

        _mediator.Send(Arg.Do<SimulateCdbCommand>(cmd => capturedCommand = cmd), Arg.Any<CancellationToken>())
                 .Returns(new SimulateCdbResponse { StatusCode = 200, Success = true });

        await CreateController().Simulate(12, 220.55m);

        Assert.NotNull(capturedCommand);
        Assert.Equal(220.55, capturedCommand.InitialValue, precision: 2);
        Assert.Equal((uint)12, capturedCommand.Months);
    }

    [Theory]
    [InlineData(0u, 220.00)]
    [InlineData(10u, -50.00)]
    public async Task Simulate_WithInvalidValues_ShouldStillReturnHandledResponse(uint months, decimal initial)
    {
        var response = new SimulateCdbResponse { StatusCode = 400, Message = "Valor inválido", Success = false };

        _mediator.Send(Arg.Any<SimulateCdbCommand>(), Arg.Any<CancellationToken>())
                 .Returns(response);

        var result = await CreateController().Simulate(months, initial);
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal(response, badRequest.Value);
    }

    [Fact]
    public async Task Simulate_WhenMessageIsNull_ShouldReturnStatusOnly()
    {
        var response = new SimulateCdbResponse { StatusCode = 500, Message = null!, Success = false };

        _mediator.Send(Arg.Any<SimulateCdbCommand>(), Arg.Any<CancellationToken>())
                 .Returns(response);

        var result = await CreateController().Simulate(3, 100m);
        var content = Assert.IsType<ContentResult>(result);

        Assert.Equal(500, content.StatusCode);
        Assert.Null(content.Content);
    }
    [Fact]
    public async Task Simulate_SuccessfulResponse_ShouldReturnOk()
    {
        var response = new SimulateCdbResponse
        {
            StatusCode = 200,
            Success = true,
            Message = "Tudo certo"
        };

        _mediator.Send(Arg.Any<SimulateCdbCommand>(), default).Returns(response);

        var result = await CreateController().Simulate(10, 100m);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, ok.Value);
    }
    [Fact]
    public async Task Simulate_ResponseWithNullMessageAndUnknownStatus_ShouldReturnEmptyContent()
    {
        var response = new SimulateCdbResponse
        {
            StatusCode = 007,
            Success = false,
            Message = null!
        };

        _mediator.Send(Arg.Any<SimulateCdbCommand>(), default).Returns(response);

        var result = await CreateController().Simulate(5, 10m);
        var content = Assert.IsType<ContentResult>(result);

        Assert.Equal(007, content.StatusCode);
        Assert.Null(content.Content);
    }
    [Fact]
    public async Task Simulate_WhenUnhandledExceptionThrown_ShouldReturnInternalServerError()
    {
        _mediator.Send(Arg.Any<SimulateCdbCommand>(), default)
                 .Throws(new NullReferenceException("Null exploded"));

        var result = await CreateController().Simulate(1, 1);

        var content = Assert.IsType<ContentResult>(result);
        Assert.Equal(500, content.StatusCode);
        Assert.Equal("Null exploded", content.Content);
    }
}
