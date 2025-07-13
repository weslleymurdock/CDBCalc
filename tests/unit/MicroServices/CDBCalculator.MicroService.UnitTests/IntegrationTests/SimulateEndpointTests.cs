using CDBCalculator.MicroService.UnitTests.Common;

namespace CDBCalculator.MicroService.UnitTests.IntegrationTests;

public class SimulateEndpointTests
{
    [Fact]
    public async Task SimulateRoute_ShouldRespondSuccessfully()
    {
        var client = MicroServiceFactory.CreateClient();

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("Months", "6"),
            new KeyValuePair<string, string>("InitialValue", "1000")
        });

        var response = await client.PostAsync("/cdb/simulate", content);

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        Assert.Contains("statusCode", json);
        Assert.Contains("message", json);


    }

}
