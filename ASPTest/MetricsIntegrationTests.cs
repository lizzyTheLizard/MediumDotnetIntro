using ASP;

using Microsoft.AspNetCore.Mvc.Testing;

namespace ASPTest;

public class MetricsIntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Base()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/Metrics");
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/plain; charset=utf-8; version=0.0.4", response.Content.Headers.ContentType?.ToString());
        var stringResponse = await response.Content.ReadAsStringAsync();
        Assert.Contains("http_server_active_requests", stringResponse);
        Assert.Contains("example_asp_apicounter", stringResponse);
    }
}
