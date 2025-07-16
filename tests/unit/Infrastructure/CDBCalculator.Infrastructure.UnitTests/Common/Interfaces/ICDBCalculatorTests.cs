using Domain.Business.Records;
using Infrastructure.Common.Interfaces;
using Moq;

namespace CDBCalculator.Infrastructure.UnitTests.Common.Interfaces;

public class ICDBCalculatorTests
{
    [Fact]
    public void Interface_Calculate_ShouldBeCalledOnce()
    {
        var mock = new Mock<ICdbCalculator>();
        var title = new Cdb(1000.0, 6);

        mock.Setup(x => x.Calculate(title)).Returns((1100.0, 900.0));

        var (totalGross, totalNet) = mock.Object.Calculate(title);

        Assert.Equal(1100.0, totalGross);
        Assert.Equal(900.0, totalNet);
        mock.Verify(x => x.Calculate(title), Times.Once);
    }


}