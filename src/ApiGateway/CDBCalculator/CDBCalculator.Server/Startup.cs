using System.Diagnostics.CodeAnalysis;
using Yarp.ReverseProxy;

namespace CDBCalculator.Server;
[ExcludeFromCodeCoverage]
/// <summary>
/// Configures services and middleware for the application.
/// </summary>
/// <remarks>The <c>Startup</c> class is responsible for setting up the application's dependency injection, 
/// middleware pipeline, and other configurations. It defines methods for registering services  and configuring the
/// request processing pipeline.  Use the <c>ConfigureServices</c> method to register application services, such as
/// controllers,  CORS policies, Swagger, reverse proxy, and SPA static files. Use the <c>Configure</c> method  to
/// define the middleware pipeline, including routing, static file handling, Swagger UI, and SPA  fallback
/// behavior.</remarks>
/// <param name="config"></param>
/// <param name="env"></param>
public class Startup(IConfiguration config, IWebHostEnvironment env)
{
    /// <summary>
    /// Configures services for the application.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = env.IsDocker() || env.IsProduction() ? "wwwroot" : "cdbcalculator.client/dist/cdbcalculator.client";
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowSPA", policy =>
                policy.WithOrigins("https://localhost:5001")
                      .AllowAnyHeader()
                      .AllowAnyMethod());
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddReverseProxy()
                .LoadFromConfig(config.GetSection("ReverseProxy"));
    }

    /// <summary>
    /// Configures the middleware pipeline for the application. 
    /// </summary>
    /// <param name="app">The application to configure</param>
    public void Configure(IApplicationBuilder app)
    {
        app.UseDefaultFiles();
        
        app.UseStaticFiles();
        
        app.UseSpaStaticFiles();

        app.UseSwagger();
        
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint(config.GetSection("MicroServices:CDBCalc:Swagger:Uri").Value, config.GetSection("MicroServices:CDBCalc:Swagger:Name").Value);
            c.RoutePrefix = "swagger";
        });

        app.UseCors("AllowSPA");

        app.UseRouting();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapReverseProxy();
            endpoints.MapFallbackToFile("index.html");
        });

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "../cdbcalculator.client";
        });
    }
}