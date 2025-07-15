using Application.CQRS.Handlers.Commands;
using Application.CQRS.Requests.Commands; 
using Domain.Common.Exceptions.CDBCalculator;
using Infrastructure.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using FluentValidation;
using FluentValidation.Results;

namespace CDBCalculator.Application.UnitTests.CQRS.Handlers.Commands;

public class SimulateCDBCommandHandlerTests
{
    private readonly Mock<ILogger<SimulateCdbCommandHandler>> _loggerMock = new();
    private readonly Mock<IValidator<SimulateCdbCommand>> _validatorMock = new();
    private readonly Mock<ICdbCalculator> _simulatorMock = new();

    private SimulateCdbCommandHandler CreateHandler() =>
        new(_loggerMock.Object, _validatorMock.Object, _simulatorMock.Object);

    [Fact]
    public async Task Handle_ShouldReturnSuccessResponse_WhenValid()
    {
        var command = new SimulateCdbCommand { InitialValue = 1000, Months = 6 };
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _simulatorMock.Setup(s => s.Calculate(It.IsAny<Domain.Business.Records.Cdb>()))
            .Returns((1100.0, 990.0));

        var handler = CreateHandler();
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Ok", result.Message);
        Assert.Equal(1100.0, result.Gross);
        Assert.Equal(990.0, result.Net);
    }

    [Fact]
    public async Task Handle_ShouldReturn422Response_WhenValidationFails()
    {
        var command = new SimulateCdbCommand { InitialValue = -1, Months = 0 };
        var failures = new List<ValidationFailure>
        {
            new(nameof(command.InitialValue), "Valor inválido"),
            new(nameof(command.Months), "Meses inválidos")
        };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        var handler = CreateHandler();
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal(422, result.StatusCode);
        Assert.Contains("Valor inválido", result.Message);
        Assert.Contains("Meses inválidos", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturn400Response_WhenCDBExceptionIsThrown()
    {
        var command = new SimulateCdbCommand { InitialValue = 1000, Months = 6 };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _simulatorMock.Setup(s => s.Calculate(It.IsAny<Domain.Business.Records.Cdb>()))
            .Throws(new CdbException("Erro Cdb"));

        var handler = CreateHandler();
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Erro Cdb", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturn500Response_WhenUnexpectedExceptionIsThrown()
    {
        var command = new SimulateCdbCommand { InitialValue = 1000, Months = 6 };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _simulatorMock.Setup(s => s.Calculate(It.IsAny<Domain.Business.Records.Cdb>()))
            .Throws(new InvalidOperationException("Falha interna"));

        var handler = CreateHandler();
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Falha interna", result.Message);
    }
}