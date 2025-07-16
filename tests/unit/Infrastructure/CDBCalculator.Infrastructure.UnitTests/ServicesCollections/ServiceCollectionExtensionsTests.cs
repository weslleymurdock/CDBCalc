using Infrastructure;
using Infrastructure.Common.Interfaces;
using InfraCalc = Infrastructure.Services.CDBCalculator.CdbCalculator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace CDBCalculator.Infrastructure.UnitTests.ServicesCollections;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddInfrastructure_ShouldRegisterICDBCalculator()
    {
        var services = new ServiceCollection(); 

        services.AddInfrastructure();
        var provider = services.BuildServiceProvider();

        var instance = provider.GetService<ICdbCalculator>();

        Assert.NotNull(instance);
        Assert.IsType<InfraCalc>(instance);
    }
}