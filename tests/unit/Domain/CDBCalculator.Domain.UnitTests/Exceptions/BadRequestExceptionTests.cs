using Domain.Common.Exceptions.Http;
using Microsoft.AspNetCore.Http;
namespace CDBCalculator.Domain.UnitTests.Exceptions;

public class BadRequestExceptionTests
{
    [Fact]
    public void ShouldReturnDefaultMessageAndStatus()
    {
        var ex = new BadRequestException();
        Assert.Equal("Bad Request", ex.Message);
        Assert.Equal(StatusCodes.Status400BadRequest, ex.StatusCode);
    }

    [Fact]
    public void ShouldAllowCustomMessage()
    {
        var ex = new BadRequestException("Campos inválidos");
        Assert.Equal("Campos inválidos", ex.Message);
        Assert.Equal(StatusCodes.Status400BadRequest, ex.StatusCode);
    }
}