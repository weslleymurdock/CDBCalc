using Infrastructure.Common.Interfaces;
using Infrastructure.Services.CDBCalculator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<ICdbCalculator, CdbCalculator>();
        return services;
    }
}
