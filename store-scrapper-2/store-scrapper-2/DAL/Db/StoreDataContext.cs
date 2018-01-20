using Microsoft.EntityFrameworkCore;

namespace store_scrapper_2.DAL.Db
{
  public class StoreDataContext : DbContext
  {
    private readonly string _connectionString;
    private readonly bool _loggingEnabled;

    internal StoreDataContext(string connectionString, bool loggingEnabled)
    {
      _connectionString = connectionString;
      _loggingEnabled = loggingEnabled;
    }

    public DbSet<Store> Stores { get; set; }
    public DbSet<Zip> Zips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      ConfigureLogging(optionsBuilder);
      optionsBuilder.UseNpgsql(_connectionString);
    }

    private void ConfigureLogging(DbContextOptionsBuilder optionsBuilder) => 
      optionsBuilder.UseLoggerFactory(DataContextLoggerProvider.CreateFactory(_loggingEnabled));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Store>()
        .HasKey(_ => new { _.StoreNumber, _.SatelliteNumber });
      modelBuilder.Entity<Store>()
        .HasIndex(_ => new { _.StoreNumber, _.SatelliteNumber });
    }
  }
}