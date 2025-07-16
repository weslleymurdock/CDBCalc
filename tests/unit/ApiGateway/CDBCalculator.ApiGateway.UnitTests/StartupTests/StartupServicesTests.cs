using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using CDBCalculator.Server;

namespace CDBCalculator.ApiGateway.UnitTests.StartupTests;

public class StartupServicesTests
{ 

    [Fact]
    public void CorsPolicy_ShouldBeRegistered()
    {
        var env = Substitute.For<IWebHostEnvironment>();
        var config = new ConfigurationBuilder().Build();
        var startup = new Server.Startup(config, env);

        var services = new ServiceCollection();
        startup.ConfigureServices(services);

        var provider = services.BuildServiceProvider();
        var corsProvider = provider.GetService<ICorsPolicyProvider>();

        Assert.NotNull(corsProvider);
    }

}

