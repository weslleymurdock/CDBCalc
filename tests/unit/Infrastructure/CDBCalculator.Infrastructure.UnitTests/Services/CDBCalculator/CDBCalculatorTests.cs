using Domain.Business.Records; 
using Domain.Common.Exceptions.CDBCalculator;
using Infrastructure.Services.CDBCalculator;
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
    [Fact]
    public void Calculate_WithNetValueInvalid_ThrowsOnInvalidNet()
    {
        // Força gross = NaN e net = NaN
        var service = new CdbCalculator();
        var gross = double.NaN;

        var result = Record.Exception(() => service.Calculate(new Cdb(gross, 12)));

        Assert.IsType<CdbException>(result);
        Assert.Contains("grossValue", result.Message);
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

    [Fact]
    public void Gross_ShouldMatchManualCalculation()
    {
        double vi = 1000;
        uint months = 12;
        double rate = Cdb.CDI * Cdb.TB;
        double expected = vi;

        for (int i = 0; i < months; i++)
            expected *= (1 + rate);

        double gross = InfraCalculator.Gross(vi, months);

        Assert.Equal(Math.Round(expected, 5), Math.Round(gross, 5));
    }

    [Fact]
    public void Taxes_WithZeroRentability_ReturnsZero()
    {
        var result = InfraCalculator.Taxes(6, 1000.0, 1000.0);
        Assert.Equal(0.0, result);
    }

    [Fact]
    public void Calculate_Gross_NaN_ThrowsCdbException()
    {
        var service = new InfraCalculator();
        var cdb = new Cdb(double.NaN, 12); // Força gross = NaN
        var ex = Assert.Throws<CdbException>(() => service.Calculate(cdb));
        Assert.Contains("grossValue", ex.Message);
    }
}