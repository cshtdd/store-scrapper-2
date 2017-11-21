using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2.DAL;

namespace store_scrapper_2.Configuration
{
  public static class DbMigrator
  { 
    public static async Task MigrateAsync()
    {
      await new StoreDataContext().Database.MigrateAsync();
    }
  }
}