using CDBCalculator.MicroService.UnitTests.Common;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace CDBCalculator.MicroService.UnitTests.ProgramTests;
public class ProgramFunctionalTests
{
    [Theory]
    [InlineData("Development", "/swagger/index.html", true)]
    [InlineData("Production", "/swagger/index.html", true)]
    [InlineData("Production", "/health", true)]
    public async Task Endpoint_ShouldRespondByEnvironment(string env, string endpoint, bool shouldSucceed)
    {
        var client = MicroServiceFactory.CreateClient(env);
        var response = await client.GetAsync(endpoint);

        if (shouldSucceed)
            response.EnsureSuccessStatusCode();
        else
            Assert.False(response.IsSuccessStatusCode);
    }
    [Fact]
    public async Task Health_ShouldReturnValidPayload()
    {
        var client = MicroServiceFactory.CreateClient("Development");
        var response = await client.GetAsync("/health");

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("Healthy", body);
        Assert.Contains("timestamp", body);
    }

}


