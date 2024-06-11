using EntityFramework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkTest;

// Needed for unit tests to use the test db context instead of the real one

public class DbContextFactory : IDisposable
{
    private readonly SqliteConnection _connection = new ("Filename=:memory:");
    private bool _disposed = false;

    public DbContextFactory()
    {
        _connection.Open();
        ResetDatabase();
    }

    public void ResetDatabase()
    {
        using var context = CreateContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
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

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _connection.Dispose();
            }

            _disposed = true;
        }
    }
}
