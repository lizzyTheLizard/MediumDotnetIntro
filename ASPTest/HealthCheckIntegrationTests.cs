using System;
using System.Net;

using ASP;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ASPTest
{
    public class HealthCheckIntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task Healthy()
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync("/Health");
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/plain", response.Content.Headers.ContentType?.ToString());
            Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task Unhealthy()
        {
            var client = factory.WithWebHostBuilder(builder => 
                builder.ConfigureServices(services => 
                    services.AddHealthChecks().AddCheck<FailingHealthCheck>("TestDummy")
            )).CreateClient();
            var response = await client.GetAsync("/Health");
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
            Assert.Equal("text/plain", response.Content.Headers.ContentType?.ToString());
            Assert.Equal("Unhealthy", await response.Content.ReadAsStringAsync());

        }
    }

    file class FailingHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("Broken while Testing"));
        }
    }
}