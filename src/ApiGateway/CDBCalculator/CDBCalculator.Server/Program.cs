using System.Diagnostics.CodeAnalysis;

namespace CDBCalculator.Server;
[ExcludeFromCodeCoverage]
/// <summary>
/// Provides the entry point for the application and methods to configure and build the host.
/// </summary>
/// <remarks>This class contains the <see cref="Main"/> method, which serves as the application's entry point, 
/// and the <see cref="CreateHostBuilder(string[])"/> method, which configures the application's host.</remarks>
public class Program
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Program"/> class.
    /// </summary>
    /// <remarks>This constructor is protected to prevent direct instantiation of the <see cref="Program"/>
    /// class. It is intended to be used by derived classes.</remarks>
    protected Program()
    {
        
    }
    /// <summary>
    /// The entry point of the application.
    /// </summary>
    /// <param name="args">An array of command-line arguments passed to the application.</param>
    private static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    /// <summary>
    /// Creates and configures a default <see cref="IHostBuilder"/> instance for the application.
    /// </summary>
    /// <remarks>This method initializes the host builder with default configurations, including support for 
    /// configuration files, logging, and dependency injection. It also configures the web host to use  the <see
    /// cref="Startup"/> class for application startup logic.</remarks>
    /// <param name="args">An array of command-line arguments passed to the application.</param>
    /// <returns>An <see cref="IHostBuilder"/> instance configured with default settings and web host defaults.</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
