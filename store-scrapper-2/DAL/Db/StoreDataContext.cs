using Microsoft.EntityFrameworkCore;
using DebugEFCore;
using store_scrapper_2.Logging;

namespace store_scrapper_2.DAL.Db
{
  public class StoreDataContext : DbContext
  {
    private readonly string _connectionString;
    private readonly bool _loggingEnabled;

    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
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
      
      Logger.LogDebug("OnConfiguring", "ConnectionString", _connectionString);
      
      optionsBuilder.UseNpgsql(_connectionString);
    }

    private void ConfigureLogging(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.EnableLogging(_loggingEnabled);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Store>()
        .HasKey(_ => new { _.StoreNumber, _.SatelliteNumber });
      modelBuilder.Entity<Store>()
        .HasIndex(_ => new { _.StoreNumber, _.SatelliteNumber });
    }
  }
}