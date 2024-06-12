using System.Net;

using ASP;

using Microsoft.AspNetCore.Mvc.Testing;

namespace ASPTest;

public class ControllerIntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetAll()
    {
        var database = factory.Services.GetService<ExampleDatabase>()!;
        await database.Clear();
        var example = await database.Create(new Example(Guid.NewGuid(), new DateOnly(2022, 1, 1), 0, "In Short: Test"));
        var client = factory.CreateClient();

        var response = await client.GetAsync("/Example");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal([example], await response.Content.ReadFromJsonAsync<ICollection<Example>>());
    }


    [Fact]
    public async Task GetAllEmpty()
    {
        var database = factory.Services.GetService<ExampleDatabase>()!;
        await database.Clear();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/Example");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal([], await response.Content.ReadFromJsonAsync<ICollection<Example>>());
    }

    [Fact]
    public async Task GetForDate()
    {
        var database = factory.Services.GetService<ExampleDatabase>()!;
        await database.Clear();
        var example = await database.Create(new Example(Guid.NewGuid(), new DateOnly(2022, 1, 1), 0, "In Short: Test"));
        var client = factory.CreateClient();

        var response = await client.GetAsync("/Example/ByDate/2022-01-01");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal([example], await response.Content.ReadFromJsonAsync<ICollection<Example>>());
    }


    [Fact]
    public async Task GetForDateEmpty()
    {
        var database = factory.Services.GetService<ExampleDatabase>()!;
        await database.Clear();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/Example/ByDate/2022-01-01");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal([], await response.Content.ReadFromJsonAsync<ICollection<Example>>());
    }

    [Fact]
    public async Task GetForId()
    {
        var database = factory.Services.GetService<ExampleDatabase>()!;
        var example = await database.Create(new Example(Guid.NewGuid(), new DateOnly(2022, 1, 1), 0, "In Short: Test"));
        var client = factory.CreateClient();

        var response = await client.GetAsync($"/Example/{example.Id}");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal(example, await response.Content.ReadFromJsonAsync<Example>());
    }


    [Fact]
    public async Task GetForIdNonExisting()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync($"/Example/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task Post()
    {
        var database = factory.Services.GetService<ExampleDatabase>()!;
        var example = new Example(Guid.NewGuid(), new DateOnly(2022, 1, 1), 0, "In Short: Test");
        var client = factory.CreateClient();

        var response = await client.PostAsync("/Example", JsonContent.Create(example));
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal(example, await response.Content.ReadFromJsonAsync<Example>());
        Assert.Equal(example, await database.GetForId(example.Id));
    }

    [Fact]
    public async Task PostInvalid()
    {
        var database = factory.Services.GetService<ExampleDatabase>()!;
        var example = new Example(Guid.NewGuid(), new DateOnly(1990, 1, 1), 0, "In Short: Test");
        var client = factory.CreateClient();

        var response = await client.PostAsync("/Example", JsonContent.Create(example));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Null(await database.GetForId(example.Id));
    }

    [Fact]
    public async Task PostExisting()
    {
        var database = factory.Services.GetService<ExampleDatabase>()!;
        var example = await database.Create(new Example(Guid.NewGuid(), new DateOnly(2022, 1, 1), 0, "In Short: Test"));
        var updatedExample = example with { Date = new DateOnly(2022, 1, 2) };
        var client = factory.CreateClient();

        var response = await client.PostAsync("/Example", JsonContent.Create(updatedExample));
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal(updatedExample, await response.Content.ReadFromJsonAsync<Example>());
        Assert.Equal(updatedExample, await database.GetForId(example.Id));
    }

    [Fact]
    public async Task Put()
    {
        var database = factory.Services.GetService<ExampleDatabase>()!;
        var example = await database.Create(new Example(Guid.NewGuid(), new DateOnly(2022, 1, 1), 0, "In Short: Test"));
        var updatedExample = example with { Date = new DateOnly(2022, 1, 2) };
        var client = factory.CreateClient();

        var response = await client.PutAsync($"/Example/{updatedExample.Id}", JsonContent.Create(updatedExample));
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal(updatedExample, await response.Content.ReadFromJsonAsync<Example>());
        Assert.Equal(updatedExample, await database.GetForId(example.Id));
    }

    [Fact]
    public async Task PutInvalid()
    {
        var database = factory.Services.GetService<ExampleDatabase>()!;
        var example = await database.Create(new Example(Guid.NewGuid(), new DateOnly(2022, 1, 1), 0, "In Short: Test"));
        var updatedExample = example with { Date = new DateOnly(1990, 1, 2) };
        var client = factory.CreateClient();

        var response = await client.PutAsync($"/Example/{updatedExample.Id}", JsonContent.Create(updatedExample));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal(example, await database.GetForId(example.Id));
    }

    [Fact]
    public async Task PutNonExisting()
    {
        var database = factory.Services.GetService<ExampleDatabase>()!;
        var example = new Example(Guid.NewGuid(), new DateOnly(2022, 1, 1), 0, "In Short: Test");
        var client = factory.CreateClient();

        var response = await client.PutAsync($"/Example/{example.Id}", JsonContent.Create(example));
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Null(await database.GetForId(example.Id));

    }


    [Fact]
    public async Task Delete()
    {
        var database = factory.Services.GetService<ExampleDatabase>()!;
        var example = await database.Create(new Example(Guid.NewGuid(), new DateOnly(2022, 1, 1), 0, "In Short: Test"));
        var client = factory.CreateClient();

        var response = await client.DeleteAsync($"/Example/{example.Id}");
        response.EnsureSuccessStatusCode();
        Assert.Null(await database.GetForId(example.Id));
    }


    [Fact]
    public async Task DeleteNonExisting()
    {
        var client = factory.CreateClient();

        var response = await client.DeleteAsync($"/Example/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

}
