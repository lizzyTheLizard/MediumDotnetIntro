using Microsoft.EntityFrameworkCore;

namespace EntityFramework;

public class Program
{
    public static void Main(string[] args)
    {
        var programm = new Program();
        programm.Run(args);
    }

    private void Run(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        //Provide the DBConext
        builder.Services.AddDbContext<ExampleDbContext>(o => o.UseSqlite($"Data Source={GetDbPath()}"));
        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }

    private static string GetDbPath()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        return Path.Join(path, "example.db");
    }
}
