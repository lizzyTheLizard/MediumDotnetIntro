using System.Net;

using EntityFramework;


namespace EntityFrameworkTest;

public class ExampleControllerIntegrationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task GetAll()
    {
        var client = factory.CreateClient();
        factory.ResetDatabase();
        var id = Guid.NewGuid();
        var example = new Example { Id = id, Date = DateOnly.FromDateTime(DateTime.Now), Value = 3, SubExamples = [], Note = "Test" };

        using (var database = factory.CreateContext())
        {
            database.Examples.Add(example);
            await database.SaveChangesAsync();
        }

        var response = await client.GetAsync("/Example");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Collection((await response.Content.ReadFromJsonAsync<ICollection<Example>>())!, e => e.Equals(example));
    }


    [Fact]
    public async Task GetAllEmpty()
    {
        var client = factory.CreateClient();
        factory.ResetDatabase();

        var response = await client.GetAsync("/Example");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Empty((await response.Content.ReadFromJsonAsync<ICollection<Example>>())!);
    }

    [Fact]
    public async Task GetForDate()
    {
        var client = factory.CreateClient();
        factory.ResetDatabase();
        var id = Guid.NewGuid();
        var example = new Example { Id = id, Date = new DateOnly(2022, 1, 1), Value = 3, SubExamples = [], Note = "Test" };

        using (var database = factory.CreateContext())
        {
            database.Examples.Add(example);
            await database.SaveChangesAsync();
        }

        var response = await client.GetAsync("/Example/ByDate/2022-01-01");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Collection((await response.Content.ReadFromJsonAsync<ICollection<Example>>())!, e => e.Equals(example));
    }


    [Fact]
    public async Task GetForDateEmpty()
    {
        var client = factory.CreateClient();
        factory.ResetDatabase();

        var response = await client.GetAsync("/Example/ByDate/2022-01-01");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Empty((await response.Content.ReadFromJsonAsync<ICollection<Example>>())!);
    }

    [Fact]
    public async Task GetForId()
    {
        var client = factory.CreateClient();
        factory.ResetDatabase();
        var id = Guid.NewGuid();
        var example = new Example { Id = id, Date = new DateOnly(2022, 1, 1), Value = 3, SubExamples = [], Note = "Test" };

        using (var database = factory.CreateContext())
        {
            database.Examples.Add(example);
            await database.SaveChangesAsync();
        }

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
    }

    [Fact]
    public async Task Post()
    {
        var client = factory.CreateClient();
        factory.ResetDatabase();
        var id = Guid.NewGuid();
        var example = new Example { Id = id, Date = new DateOnly(2022, 1, 1), Value = 3, SubExamples = [], Note = "Test" };

        var response = await client.PostAsync("/Example", JsonContent.Create(example));
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal(example, await response.Content.ReadFromJsonAsync<Example>());
        Assert.Matches($"http://localhost\\d*/Example/{id}", response.Headers.Location?.ToString());
        using (var database = factory.CreateContext())
        {
            Assert.Equal(example, database.Examples.Single(e => e.Id == example.Id));
        }
    }

    [Fact]
    public async Task PostExisting()
    {
        var client = factory.CreateClient();
        factory.ResetDatabase();
        var id = Guid.NewGuid();
        var example = new Example { Id = id, Date = new DateOnly(2022, 1, 1), Value = 3, SubExamples = [], Note = "Test" };
        var updatedExample = new Example { Id = id, Date = new DateOnly(2022, 1, 2), Value = 3, SubExamples = [], Note = "Test" };

        using (var database = factory.CreateContext())
        {
            database.Examples.Add(example);
            await database.SaveChangesAsync();
        }

        var response = await client.PostAsync("/Example", JsonContent.Create(updatedExample));
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal(updatedExample, await response.Content.ReadFromJsonAsync<Example>());
        Assert.Matches($"http://localhost\\d*/Example/{id}", response.Headers.Location?.ToString());
        using (var database = factory.CreateContext())
        {
            Assert.Equal(updatedExample, database.Examples.Single(e => e.Id == example.Id));
        }
    }

    [Fact]
    public async Task Put()
    {
        var client = factory.CreateClient();
        factory.ResetDatabase();
        var id = Guid.NewGuid();
        var example = new Example { Id = id, Date = new DateOnly(2022, 1, 1), Value = 3, SubExamples = [], Note = "Test" };
        var updatedExample = new Example { Id = id, Date = new DateOnly(2022, 1, 2), Value = 3, SubExamples = [], Note = "Test" };

        using (var database = factory.CreateContext())
        {
            database.Examples.Add(example);
            await database.SaveChangesAsync();
        }

        var response = await client.PutAsync($"/Example/{id}", JsonContent.Create(updatedExample));
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal(updatedExample, await response.Content.ReadFromJsonAsync<Example>());
        using var resultDb = factory.CreateContext();
        Assert.Equal(updatedExample, resultDb.Examples.Single(e => e.Id == example.Id));
    }

    [Fact]
    public async Task PutNonExisting()
    {
        var client = factory.CreateClient();
        factory.ResetDatabase();
        var id = Guid.NewGuid();
        var example = new Example { Id = id, Date = new DateOnly(2022, 1, 1), Value = 3, SubExamples = [], Note = "Test" };

        var response = await client.PutAsync($"/Example/{example.Id}", JsonContent.Create(example));
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        using (var database = factory.CreateContext())
        {
            Assert.Null(database.Examples.FirstOrDefault(e => e.Id == example.Id));
        }
    }


    [Fact]
    public async Task Delete()
    {
        var client = factory.CreateClient();
        factory.ResetDatabase();
        var id = Guid.NewGuid();
        var example = new Example { Id = id, Date = new DateOnly(2022, 1, 1), Value = 3, SubExamples = [], Note = "Test" };

        using (var database = factory.CreateContext())
        {
            database.Examples.Add(example);
            await database.SaveChangesAsync();
        }

        var response = await client.DeleteAsync($"/Example/{id}");
        response.EnsureSuccessStatusCode();
        using (var database = factory.CreateContext())
        {
            Assert.Null(database.Examples.FirstOrDefault(e => e.Id == id));
        }
    }


    [Fact]
    public async Task DeleteNonExisting()
    {
        var client = factory.CreateClient();

        var response = await client.DeleteAsync($"/Example/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
