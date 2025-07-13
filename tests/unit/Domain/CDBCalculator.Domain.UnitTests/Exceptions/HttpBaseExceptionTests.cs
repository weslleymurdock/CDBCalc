using Domain.Common.Exceptions.Http.Base;
using Microsoft.AspNetCore.Http;

namespace CDBCalculator.Domain.UnitTests.Exceptions;

public class HttpBaseExceptionTests
{
    [Fact]
    public void ShouldHaveDefaultStatus()
    {
        var ex = new HttpBaseException();
        Assert.Equal(StatusCodes.Status500InternalServerError, ex.StatusCode);
        Assert.Equal("An internal server error occurred", ex.Message);
    }

    [Fact]
    public void ShouldAllowCustomMessageAndStatus()
    {
        var ex = new HttpBaseException("Erro customizado", 418);
        Assert.Equal(418, ex.StatusCode);
        Assert.Equal("Erro customizado", ex.Message);
    }
}