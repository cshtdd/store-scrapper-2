using Microsoft.EntityFrameworkCore;

namespace store_scrapper_2.DAL
{
  public class StoreDataContext : DbContext
  {
    private readonly string _connectionString;
    private readonly SupportedDatabases _dbProvider;

    internal StoreDataContext(string connectionString, SupportedDatabases dbProvider)
    {
      _connectionString = connectionString;
      _dbProvider = dbProvider;
    }

    public DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (_dbProvider == SupportedDatabases.Postgres)
      {
        optionsBuilder.UseNpgsql(_connectionString);
        return;
      }

      optionsBuilder.UseSqlite(_connectionString);
    }
  }
}