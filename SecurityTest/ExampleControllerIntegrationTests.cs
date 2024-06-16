using System.Net;
using System.Net.Http.Headers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

using Security;


namespace SecurityTest;

public class ExampleControllerIntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Unauthenticated()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/WeatherForecast");
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.StartsWith("https://login.microsoftonline.com/", response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task BasicAuth()
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Basic QWRtaW46U2VjcmV0");
        var response = await client.GetAsync("/WeatherForecast");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.StartsWith("[{\"date\":\"2", await response.Content.ReadAsStringAsync());
    }

    //TODO: Cookie and OIDC-Mock

    [Fact]
    public async Task UnauthenticatedPublic()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/WeatherForecast/Public");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.StartsWith("[{\"date\":\"2", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task MockAuthentication()
    {
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication(o => o.DefaultScheme = "TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
            });
        }).CreateClient();

        var response = await client.GetAsync("/WeatherForecast");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.StartsWith("[{\"date\":\"2", await response.Content.ReadAsStringAsync());
    }
}
