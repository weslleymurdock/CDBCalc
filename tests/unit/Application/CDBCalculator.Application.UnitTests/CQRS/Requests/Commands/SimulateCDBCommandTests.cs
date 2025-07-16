using Xunit;
using Application.CQRS.Requests.Commands;

namespace CDBCalculator.Application.UnitTests.CQRS.Requests.Commands;

public class SimulateCDBCommandTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaults()
    {
        var command = new SimulateCdbCommand();

        Assert.Equal(1u, command.Months);
        Assert.Equal(0.00, command.InitialValue);
    }

    [Fact]
    public void Properties_ShouldAcceptCustomValues()
    {
        var command = new SimulateCdbCommand
        {
            Months = 12,
            InitialValue = 1500.50
        };

        Assert.Equal(12u, command.Months);
        Assert.Equal(1500.50, command.InitialValue);
    }

    [Theory]
    [InlineData(0u, 0.0)]
    [InlineData(6u, 500.0)]
    [InlineData(24u, 10000.0)]
    public void CanInitializeVariousValues(uint months, double value)
    {
        var command = new SimulateCdbCommand { Months = months, InitialValue = value };

        Assert.Equal(months, command.Months);
        Assert.Equal(value, command.InitialValue);
    }
}