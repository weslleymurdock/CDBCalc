using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Infrastructure;
namespace Application;
/// <summary>
/// Adds application-specific services to the specified <see cref="IServiceCollection"/>.
/// </summary>
/// <remarks>This method registers infrastructure services, MediatR handlers, and validators from the assembly
/// containing the <see cref="ServiceCollectionExtensions"/> class.</remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures and registers application-level services into the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>This method adds infrastructure services, MediatR handlers, and validators from the assembly
    /// containing <see cref="ServiceCollectionExtensions"/> to the provided <see cref="IServiceCollection"/>.</remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the application services will be added.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance with the application services registered.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddInfrastructure();
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        return services;
    }
}
