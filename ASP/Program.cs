using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ASP;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.SetupDI();
        var app = builder.Build();
        app.SetupMiddleware();
        app.Run();
    }
}

file static class WebApplicationBuilderExtensions
{
    public static void SetupDI(this WebApplicationBuilder builder)
    {
        //Add controllers to DI
        builder.Services.AddControllers();

        //Add API-Explorer (used for Swagger) and Swagger to DI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Add DB-Dummy to DI
        builder.Services.AddSingleton<ExampleDatabase>();

        //Add Health Checks to DI
        builder.Services.AddHealthChecks()
            .AddCheck("test1", () => HealthCheckResult.Unhealthy("This is always unhealthy"))
            .AddCheck("test2", () => HealthCheckResult.Healthy("This is always healthy"));

        //Add Properties to DI
        builder.Services.Configure<ExampleOptions>(builder.Configuration.GetSection(ExampleOptions.Example));
    }

    public static void SetupMiddleware(this WebApplication app)
    {
        //Use Swagger only in Development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseMiddleware<ExampleMiddleware>();
        app.MapHealthChecks("/health");
        app.UseAuthorization();
        app.MapControllers();
    }

}
