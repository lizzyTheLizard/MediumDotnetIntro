using Microsoft.AspNetCore.HttpLogging;

using OpenTelemetry.Metrics;

namespace ASP;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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
        builder.Services.AddHttpLogging(o =>
        {
            o.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestMethod | HttpLoggingFields.ResponseStatusCode;
            o.CombineLogs = true;

        });

        //Configure OpenTelemetry
        builder.Services.AddOpenTelemetry().WithMetrics(builder =>
        {
            builder.AddPrometheusExporter();
            builder.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel", "Example");
        });

        // Normally you do not have to add a middleware to the DI, but in this case we need it to add the metric
        builder.Services.AddSingleton<ApiTagMetric>();

        var app = builder.Build();
        app.UseMiddleware<MetricsTaggingMiddleware>();
        app.UseHttpLogging();
        app.UseAuthorization();
        app.UseHealthChecks("/Health");
        app.UseOpenTelemetryPrometheusScrapingEndpoint("/Metrics");
        //Use Swagger only in Development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.MapControllers();
        app.Run();
    }
}
