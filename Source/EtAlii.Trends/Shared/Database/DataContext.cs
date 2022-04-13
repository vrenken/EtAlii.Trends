namespace EtAlii.Trends;

using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Extensions.Logging;

public class DataContext : DbContext
{
#pragma warning disable CS8618
    public DbSet<Trend> Trends { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<Diagram> Diagrams { get; init; }
    public DbSet<Layer> Layers { get; init; }
#pragma warning restore CS8618

    private readonly ILogger _logger = Log.ForContext<DataContext>();

    // The following configures EF to create a Sqlite database file as `C:\blogging.db`.
    // For Mac or Linux, change this to `/tmp/blogging.db` or any other absolute path.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var databaseFilename = Environment.OSVersion.Platform == PlatformID.Win32NT
            ? "database.db"
            : "/trends/data/database.db";
        optionsBuilder.UseSqlite($"Data Source={databaseFilename}");

#pragma warning disable CA2000
        var loggerFactory = new SerilogLoggerFactory(_logger);
        optionsBuilder.UseLoggerFactory(loggerFactory);
#pragma warning restore CA2000
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            modelBuilder.ApplyConfiguration<Entity>(typeof(EntityTypeConfiguration<>), entityType.ClrType);
        }

        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DiagramEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LayerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TrendEntityTypeConfiguration());
    }
}
