using Microsoft.OpenApi.Models;
using Application;

namespace CDBCalculator;

public class Startup(IWebHostEnvironment env)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowGateway", policy =>
                policy.WithOrigins("https://localhost:5001", "https://localhost:5003")
                      .AllowAnyMethod()
                      .AllowAnyHeader());
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Simulador de Cdb",
                Version = "v1"
            });
        });

        services.AddApplication();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseCors("AllowGateway");

        if (env.IsProduction())
        {
            app.UseHttpsRedirection();
        }

        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cdb Calculator API");
        });

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/health", () =>
                Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }));
        });
    }
}