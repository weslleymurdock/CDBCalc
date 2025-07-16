using Xunit;
using Domain.Business.Records;

namespace CDBCalculator.Domain.UnitTests.Records;

public class CDBTests
{
    [Fact]
    public void ShouldInitializeWithGivenValues()
    {
        var vi = 1000;
        var months = 6u;
        var cdb = new Cdb(vi, months);

        Assert.Equal(vi, cdb.VI);
        Assert.Equal(months, cdb.Months);
    }

    [Fact]
    public void ShouldExposeStaticConstants()
    {
        Assert.Equal(1.08, Cdb.TB);
        Assert.Equal(0.009, Cdb.CDI);
    }
}