using Microsoft.EntityFrameworkCore;

namespace store_scrapper_2.DAL.Db
{
  public class StoreDataContext : DbContext
  {
    private readonly string _connectionString;

    internal StoreDataContext(string connectionString) => _connectionString = connectionString;

    public DbSet<Store> Stores { get; set; }
    public DbSet<Zip> Zips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(_connectionString);
  }
}