using ASP;

using Microsoft.AspNetCore.Mvc.Testing;

namespace ASPTest
{
    public class OptionsIntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GetAll()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync("/Other/Options");
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            Assert.Equal("{\"title\":\"Titel String 2X\",\"name\":\"Name String\"}", await response.Content.ReadAsStringAsync());
        }

        //TODO: Test for changing properties!
    }
}
