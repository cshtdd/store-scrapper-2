using Microsoft.EntityFrameworkCore;
using store;

namespace store_scrapper_2.DAL
{
  public class StoreDataContext : DbContext
  {
    public DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Data Source=stores.db");
    }
  }
}