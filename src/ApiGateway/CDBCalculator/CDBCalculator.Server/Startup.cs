using Yarp.ReverseProxy;

namespace CDBCalculator.Server;

public class Startup(IConfiguration config, IWebHostEnvironment env)
{
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

    public void Configure(IApplicationBuilder app)
    {
        app.UseDefaultFiles();
        
        app.UseStaticFiles();
        
        app.UseSpaStaticFiles();

        app.UseSwagger();
        
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint(config["MicroServices:CDBCalc:Swagger:Uri"], config["MicroServices:CDBCalc:Swagger:Name"]);
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