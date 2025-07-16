using Application.CQRS.Requests.Commands;
using Application.CQRS.Responses.CDBCalculator;
using CdbController = CDBCalculator.Controllers.CdbController;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CDBCalculator.MicroService.UnitTests.ControllersTests.CDBControllerTests;

public class CDBControllerTests
{
    private readonly Mock<ILogger<CdbController>> _loggerMock = new();
    private readonly Mock<IMediator> _mediatorMock = new();

    private CdbController CreateController() =>
        new(_loggerMock.Object, _mediatorMock.Object);

    [Theory]
    [InlineData(200, typeof(OkObjectResult))]
    [InlineData(400, typeof(BadRequestObjectResult))]
    [InlineData(422, typeof(UnprocessableEntityObjectResult))]
    [InlineData(500, typeof(ContentResult))]
    public async Task Simulate_ReturnsExpectedResponse(int statusCode, Type expectedResultType)
    {
        var response = new SimulateCdbResponse { StatusCode = statusCode, Message = "Mensagem", Success = false };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<SimulateCdbCommand>(), default))
            .ReturnsAsync(response);

        var controller = CreateController();
        var result = await controller.Simulate(6, (decimal)1000.0);

        Assert.IsType(expectedResultType, result);
    }

    [Fact]
    public async Task Simulate_WhenExceptionThrown_Returns500ContentResult()
    {
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<SimulateCdbCommand>(), default))
            .ThrowsAsync(new InvalidOperationException("Falhou"));

        var controller = CreateController();
        var result = await controller.Simulate(6, (decimal)1000.0);

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

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<SimulateCdbCommand>(), default))
            .ReturnsAsync(response);

        var controller = CreateController();
        var result = await controller.Simulate(6, (decimal)1000.0);

        var content = Assert.IsType<ContentResult>(result);
        Assert.Equal(418, content.StatusCode);
        Assert.Equal("Sou um bule de chá", content.Content);
    }

    [Fact]
    public async Task Simulate_WhenResponseIsNull_ReturnsEmptyContentResult()
    {

        _mediatorMock
            .Setup(m => m.Send<SimulateCdbResponse?>(It.IsAny<SimulateCdbCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SimulateCdbResponse?)null);

        var controller = CreateController();
        var result = await controller.Simulate(6, (decimal)1000.0);

        var content = Assert.IsType<ContentResult>(result);
        Assert.NotNull(content.Content); 
    }

}
