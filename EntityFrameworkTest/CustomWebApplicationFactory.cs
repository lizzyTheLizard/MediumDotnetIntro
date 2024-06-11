using System.Data.Common;

using EntityFramework;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EntityFrameworkTest;

// Needed for integration tests to use the test db context instead of the real one
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private bool _disposed = false;
    private readonly SqliteConnection _connection = new ("Filename=:memory:");

    public CustomWebApplicationFactory()
    {
        _connection.Open();
        using var context = CreateContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        ResetDatabase();
    }

    public void ResetDatabase()
    {
        using var context = CreateContext();
        context.Examples.RemoveRange(context.Examples);
        context.UserConfiguration.RemoveRange(context.UserConfiguration);
        //Add see data here if needed
        context.SaveChanges();
    }

    public ExampleDbContext CreateContext()
    {
        var contextOptions = new DbContextOptionsBuilder<ExampleDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new ExampleDbContext(contextOptions);
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _connection.Dispose();
            }
            _disposed = true;
        }
        base.Dispose(disposing);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ExampleDbContext>>();
            services.AddDbContext<ExampleDbContext>((container, options) => options.UseSqlite(_connection));
        });
        builder.UseEnvironment("Development");
    }
}
