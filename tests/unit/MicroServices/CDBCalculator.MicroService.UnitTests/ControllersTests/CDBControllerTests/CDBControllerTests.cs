using Application.CQRS.Requests.Commands;
using Application.CQRS.Responses.CDBCalculator;
using Domain.Business.Records;
using Domain.Common.Exceptions.CDBCalculator;
using Infrastructure.Common.Interfaces;
using Infrastructure.Services.CDBCalculator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Reflection.Metadata;
using CdbController = CDBCalculator.Controllers.CdbController;

namespace CDBCalculator.MicroService.UnitTests.ControllersTests.CDBControllerTests;

public class CdbControllerTests
{
    private readonly ILogger<CdbController> _logger = Substitute.For<ILogger<CdbController>>();
    private readonly ICdbCalculator _calculator = new CdbCalculator();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly CdbController _controller;

    public CdbControllerTests()
    {
        _controller = new CdbController(_logger, _mediator);
    }

    private SimulateCdbResponse CalculateResponse(double vi, uint months)
    {
        if (vi < 0.01)
        {
            return new SimulateCdbResponse
            {
                StatusCode = 422,
                Success = false,
                Message = "Valor inicial inválido"
            };
        }

        try
        {
            var cdb = new Cdb(vi, months);
            var (gross, net) = _calculator.Calculate(cdb);

            return new SimulateCdbResponse
            {
                Gross = gross,
                Net = net,
                Success = true,
                StatusCode = 200,
                Message = "Simulação bem-sucedida"
            };
        }
        catch (CdbException ex)
        {
            return new SimulateCdbResponse
            {
                StatusCode = 422,
                Success = false,
                Message = ex.Message
            };
        }
    }


    [Theory]
    [InlineData(220.00, 6, 200)]
    [InlineData(0.00, 6, 422)]
    [InlineData(220.00, 0, 422)]
    public async Task Simulate_ShouldHandleValidAndInvalidInput(decimal vi, uint months, int expectedStatus)
    {
        _mediator.Send(Arg.Any<SimulateCdbCommand>(), Arg.Any<CancellationToken>())
                 .Returns(ci =>
                 {
                     var cmd = ci.Arg<SimulateCdbCommand>();
                     return CalculateResponse(cmd.InitialValue, cmd.Months);
                 });

        var result = await _controller.Simulate(months, vi);
        var objectResult = Assert.IsAssignableFrom<IActionResult>(result);

        if (expectedStatus == 200)
        {
            var ok = Assert.IsType<OkObjectResult>(objectResult);
            var response = Assert.IsType<SimulateCdbResponse>(ok.Value);
            Assert.True(response.Success);
        }
        else
        {
            var response = objectResult switch
            {
                UnprocessableEntityObjectResult r => r.Value as SimulateCdbResponse,
                BadRequestObjectResult r => r.Value as SimulateCdbResponse,
                ContentResult r => new SimulateCdbResponse
                {
                    Message = r.Content!,
                    StatusCode = r.StatusCode ?? 500,
                    Success = false
                },
                _ => null
            };

            Assert.NotNull(response);
            Assert.False(response!.Success);
            Assert.Equal(expectedStatus, response.StatusCode);
        }
    }

    [Fact]
    public async Task Simulate_WhenBadRequestHappens_ShouldReturn400()
    {
        var response = new SimulateCdbResponse
        {
            StatusCode = 400,
            Message = "Bad Request",
            Success = false
        };
        _mediator.Send(Arg.Any<SimulateCdbCommand>(), Arg.Any<CancellationToken>())
                 .Returns(response);
        var result = await _controller.Simulate(10, 0.00m);
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        var content = Assert.IsType<SimulateCdbResponse>(objectResult.Value);
        Assert.Equal(400, content.StatusCode);
        Assert.False(content.Success);
        Assert.Equal("Bad Request", content.Message);
    }

    [Fact]
    public async Task Simulate_WhenInternalServerErrorHappens_ShouldReturn500()
    {
        var response = new SimulateCdbResponse
        {
            StatusCode = 500,
            Message = "Internal Server Error",
            Success = false
        };
        _mediator.Send(Arg.Any<SimulateCdbCommand>(), Arg.Any<CancellationToken>())
                 .Returns(response);
        var result = await _controller.Simulate(10, 1000m);
        var content = Assert.IsType<ContentResult>(result);
        Assert.Equal(500, content.StatusCode);
        Assert.Equal("Internal Server Error", content.Content);
    }

    [Fact]
    public async Task Simulate_WhenUnhandledExceptionOccurs_ShouldReturn500()
    {
        _mediator.Send(Arg.Any<SimulateCdbCommand>(), Arg.Any<CancellationToken>())
                 .Throws(new Exception("Falha geral"));

        var result = await _controller.Simulate(10, 1000m);
        var content = Assert.IsType<ContentResult>(result);
        Assert.Equal(500, content.StatusCode);
        Assert.Contains("Falha geral", content.Content);
    }

    [Fact]
    public async Task Simulate_WhenResponseIsNull_ShouldReturnNullContent()
    {
        _mediator.Send(Arg.Any<SimulateCdbCommand>(), Arg.Any<CancellationToken>())
                 .Returns((SimulateCdbResponse?)null!);

        var result = await _controller.Simulate(10, 1000m);
        var content = Assert.IsType<ContentResult>(result);
        Assert.Null(content.Content!);
    }

    [Fact]
    public async Task Simulate_ShouldHandleUnexpectedStatusCodeWithMessage()
    {
        var response = new SimulateCdbResponse
        {
            StatusCode = 418,
            Message = "Im a tea pot",
            Success = false
        };

        _mediator.Send(Arg.Any<SimulateCdbCommand>(), Arg.Any<CancellationToken>())
                 .Returns(response);

        var result = await _controller.Simulate(10, 1000m);
        var content = Assert.IsType<ContentResult>(result);
        Assert.Equal(418, content.StatusCode);
        Assert.Equal("Im a tea pot", content.Content);
    }

}

