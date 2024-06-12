using Microsoft.EntityFrameworkCore;

namespace EntityFramework;

// DBContext is the main interaction with the DB. It is a scoped bean (use it and throw it away). 
// You can define additional methods (like Add etc) directely here or just let other classes use the Properties

public class ExampleDbContext(DbContextOptions<ExampleDbContext> options) : DbContext(options)
{
    //All the entitites are provided as DbSets. Services can use Linq to access them
    //You can also Add, Update, Remove or RemoveRange from it
    //After changes, you need to call Save on the Context.
    public DbSet<Example> Examples { get; set; }
    public DbSet<UserConfiguration> UserConfiguration { get; set; }

    // Here you can define how the model is mapped to the DB
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Example>().Property(e => e.Note).HasColumnName("Note2").HasColumnType("varchar256");
        modelBuilder.Entity<Example>().HasMany(e => e.SubExamples).WithOne().IsRequired();
        modelBuilder.Entity<UserConfiguration>().Property(e => e.Name).HasConversion<string>();
        modelBuilder.Entity<UserConfiguration>().HasKey(e => e.Name);
        //This loads the example eagerly when loading a UserConfiguration
        modelBuilder.Entity<UserConfiguration>().Navigation(e => e.Example).AutoInclude();
        //This sets the Timestamp property as a row version to enable optimistic locking
        //SQlite does not support automatic values, otherwise the DB could set this as well
        modelBuilder.Entity<Example>().Property(e => e.Timestamp).IsConcurrencyToken();
    }
}
