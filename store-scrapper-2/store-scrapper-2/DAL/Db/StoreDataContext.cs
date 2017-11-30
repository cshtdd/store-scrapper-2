using Microsoft.EntityFrameworkCore;

namespace store_scrapper_2.DAL
{
  public class StoreDataContext : DbContext
  {
    private readonly string _databaseName;

    public StoreDataContext(string databaseName="stores.db")
    {
      _databaseName = databaseName;
    }

    public DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite($"Data Source={_databaseName}");
    }
  }
}