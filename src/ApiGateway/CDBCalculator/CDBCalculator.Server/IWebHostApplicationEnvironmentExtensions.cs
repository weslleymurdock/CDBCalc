namespace CDBCalculator.Server;

/// <summary>
/// Provides extension methods for <see cref="IWebHostEnvironment"/> to assist in identifying specific application
/// environments.
/// </summary>
/// <remarks>These methods are designed to simplify checks for specific environment configurations, such as
/// determining if the application is running in a Docker container.</remarks>
public static class IWebHostApplicationEnvironmentExtensions
{
    public static bool IsDocker(this IWebHostEnvironment environment) => environment.EnvironmentName.Equals("Docker", StringComparison.OrdinalIgnoreCase);
}
