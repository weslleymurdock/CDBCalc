using Domain.Business.Records; 
using Domain.Common.Exceptions.CDBCalculator;
using InfraCalculator = Infrastructure.Services.CDBCalculator.CdbCalculator;
namespace CDBCalculator.Infrastructure.UnitTests.Services.CDBCalculator;

public class CDBCalculatorTests
{
    [Fact]
    public void Calculate_WithValidInput_ReturnsRoundedGrossAndNetValues()
    {
        var service = new InfraCalculator();
        var cdb = new Cdb(1000.0, 6);
        var (gross, net) = service.Calculate(cdb);

        Assert.True(gross > net);
        Assert.Equal(Math.Round(gross, 2), gross);
        Assert.Equal(Math.Round(net, 2), net);
    }

    [Fact]
    public void Calculate_WithZeroMonths_ThrowsCDBException()
    {
        var service = new InfraCalculator();
        var cdb = new Cdb(1000.0, 0);

        Assert.Throws<CdbException>(() => service.Calculate(cdb));
    }

    [Fact]
    public void Calculate_WithExtremeInitialValue_ThrowsOnInvalidGross()
    {
        var service = new InfraCalculator();
        var cdb = new Cdb(double.MaxValue, 12);  // Pode ultrapassar valores finitos

        var ex = Assert.Throws<CdbException>(() => service.Calculate(cdb));
        Assert.Contains("grossValue", ex.Message);
    }

    [Theory]
    [InlineData(3, 0.225)]
    [InlineData(9, 0.20)]
    [InlineData(18, 0.175)]
    [InlineData(30, 0.15)]
    public void Calculate_ShouldApplyCorrectAliquota(uint months, double expectedAliquota)
    {
        var service = new InfraCalculator();
        var initial = 1000.0;
        var cdb = new Cdb(initial, months);
        var (gross, net) = service.Calculate(cdb);

        var final = gross - initial;
        var calculated = Math.Round((gross - net) / final, 3);

        Assert.Equal(expectedAliquota, calculated);
    }
}