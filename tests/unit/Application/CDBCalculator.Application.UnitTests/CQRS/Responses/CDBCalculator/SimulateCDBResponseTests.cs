using Xunit;
using Application.CQRS.Responses.CDBCalculator;

namespace CDBCalculator.Application.UnitTests.CQRS.Responses.CDBCalculator;

public class SimulateCDBResponseTests
{
    [Fact]
    public void SimulateCDBResponse_Defaults_ShouldBeCorrect()
    {
        var response = new SimulateCdbResponse();

        Assert.False(response.Success);      
        Assert.Equal(0, response.StatusCode);
        Assert.Equal(string.Empty, response.Message); 
        Assert.Equal(0.0, response.Gross);  
        Assert.Equal(0.0, response.Net);     
    }

    [Fact]
    public void SimulateCDBResponse_Properties_ShouldBeSettable()
    {
        var response = new SimulateCdbResponse
        {
            Success = true,
            StatusCode = 200,
            Message = "Simulação OK",
            Gross = 1234.56,
            Net = 987.65
        };

        Assert.True(response.Success);
        Assert.Equal(200, response.StatusCode);
        Assert.Equal("Simulação OK", response.Message);
        Assert.Equal(1234.56, response.Gross);
        Assert.Equal(987.65, response.Net);
    }
}