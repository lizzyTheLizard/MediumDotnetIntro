using Microsoft.EntityFrameworkCore;

namespace EntityFramework;

public class ExampleDbContext : DbContext
{
    public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options) {
    }

    public DbSet<Example> Examples { get; set; }
    public DbSet<UserConfiguration> UserConfiguration { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Example>().Property(e => e.Note).HasColumnName("Note2").HasColumnType("varchar256");
        modelBuilder.Entity<Example>().HasMany(e => e.SubExamples).WithOne().IsRequired();
        modelBuilder.Entity<UserConfiguration>().Property(e => e.Name).HasConversion<string>();
        modelBuilder.Entity<UserConfiguration>().HasKey(e => e.Name);
        //This loads the example eagerly when loading a UserConfiguration
        modelBuilder.Entity<UserConfiguration>().Navigation(e => e.Example).AutoInclude();
        //This sets the Timestamp property as a row version to enable optimistic locking
        modelBuilder.Entity<Example>().Property(e => e.Timestamp).IsConcurrencyToken();
    }

}