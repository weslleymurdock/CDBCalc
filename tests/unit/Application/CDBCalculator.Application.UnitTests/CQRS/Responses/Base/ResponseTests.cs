using Xunit;
using Application.CQRS.Responses.Base;

namespace CDBCalculator.Application.UnitTests.CQRS.Responses.Base;

public class ResponseTests
{
    private class TestResponse : Response { }

    [Fact]
    public void Response_Defaults_ShouldBeCorrect()
    {
        var response = new TestResponse();

        Assert.False(response.Success);              
        Assert.Equal(0, response.StatusCode);        
        Assert.Equal(string.Empty, response.Message);
    }

    [Fact]
    public void Response_Properties_ShouldBeSettable()
    {
        var response = new TestResponse
        {
            Success = true,
            StatusCode = 200,
            Message = "TestResponse Abstract OK"
        };

        Assert.True(response.Success);
        Assert.Equal(200, response.StatusCode);
        Assert.Equal("TestResponse Abstract OK", response.Message);
    }
}