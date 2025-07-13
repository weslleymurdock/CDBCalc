using Domain.Common.Exceptions.CDBCalculator;
using Domain.Common.Exceptions.Http;
using Domain.Common.Exceptions.Http.Base;
using Microsoft.AspNetCore.Http;

namespace CDBCalculator.Infrastructure.UnitTests.Common.Exceptions;

public class ExceptionTests
{
    [Fact]
    public void CDBException_ShouldStoreMessage()
    {
        var ex = new CDBException("Erro de cálculo");
        Assert.Equal("Erro de cálculo", ex.Message);
    }

    [Fact]
    public void HttpBaseException_DefaultsTo500()
    {
        var ex = new HttpBaseException();
        Assert.Equal(StatusCodes.Status500InternalServerError, ex.StatusCode);
        Assert.Equal("An internal server error occurred", ex.Message);
    }

    [Fact]
    public void HttpBaseException_CustomMessageAndStatus()
    {
        var ex = new HttpBaseException("Custom", 418);
        Assert.Equal("Custom", ex.Message);
        Assert.Equal(StatusCodes.Status418ImATeapot, ex.StatusCode);
    }

    [Fact]
    public void BadRequestException_ShouldSet400AndDefaultMessage()
    {
        var ex = new BadRequestException();
        Assert.Equal("Bad Request", ex.Message);
        Assert.Equal(StatusCodes.Status400BadRequest, ex.StatusCode);
    }

    [Fact]
    public void BadRequestException_ShouldAcceptCustomMessage()
    {
        var ex = new BadRequestException("Campo inválido");
        Assert.Equal("Campo inválido", ex.Message);
        Assert.Equal(StatusCodes.Status400BadRequest, ex.StatusCode);
    }
}

