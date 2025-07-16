using Xunit;
using Domain.Common.Exceptions.CDBCalculator;

namespace CDBCalculator.Domain.UnitTests.Exceptions;


public class CDBExceptionTests
{
    [Fact]
    public void ShouldReturnCustomMessage()
    {
        var ex = new CdbException("Erro de cálculo");
        Assert.Equal("Erro de cálculo", ex.Message);
    }
}
