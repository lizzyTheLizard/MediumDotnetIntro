namespace EntityFrameworkTest;

using EntityFramework;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

// A unit test using an in-memory SQLite database
public class ExampleControllerUnitTests(DbContextFactory dbContextFixture) : IClassFixture<DbContextFactory>
{
    private readonly static Guid Id = Guid.NewGuid();
    private readonly static Example Example = new Example { Id = Id, Date = DateOnly.FromDateTime(DateTime.Now), Value = 3, SubExamples = [], Note = "Test" };

    [Fact]
    public async Task EmptyResult()
    {
        using var context = dbContextFixture.CreateContext();
        dbContextFixture.ResetDatabase();
        var controller = new ExampleController(new NullLogger<ExampleController>(), context);

        var result = await controller.GetAll();
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);

        var result2 = await controller.GetForId(Id);
        Assert.Null(result2.Value);
        Assert.IsAssignableFrom<NotFoundResult>(result2.Result);
    }

    [Fact]
    public async Task NonEmptyResult()
    {
        dbContextFixture.ResetDatabase();
        using var context = dbContextFixture.CreateContext();
        context.Examples.Add(Example);
        context.SaveChanges();

        var controller = new ExampleController(new NullLogger<ExampleController>(), context);

        var result = await controller.GetAll();
        Assert.NotNull(result.Value);
        Assert.Collection(result.Value, e => e.Equals(Example));

        var result2 = await controller.GetForId(Id);
        Assert.Equal(Example, result2.Value);
    }

}
