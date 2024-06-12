using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ASP;

public class ExampleHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("Everythings OK"));
    }
}
