using Microsoft.AspNetCore.HttpLogging;
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
        //Add Properties to DI
        builder.Services.Configure<ExampleOptions>(builder.Configuration.GetSection(ExampleOptions.Example));

        //Add controllers to DI
        builder.Services.AddControllers();

        //Add API-Explorer (used for Swagger) and Swagger to DI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Add DB-Dummy to DI
        builder.Services.AddSingleton<ExampleDatabase>();

        //Configure Health Checks to DI
        builder.Services.AddHealthChecks().AddCheck<ExampleHealthCheck>("Sample");

        //Configure HTTP-Logging
        builder.Services.AddHttpLogging(o => { });
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
        app.UseHttpLogging();
        app.MapHealthChecks("/Health");
        app.UseAuthorization();
        app.MapControllers();
    }

}
