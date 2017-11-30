using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2.DAL;

namespace store_scrapper_2
{
  public class PersistenceInitializer : IPersistenceInitializer
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public async Task Initialize()
    {
      using (var context = new StoreDataContext())
      {
        Logger.Debug("DbMigration Started");
        await context.Database.MigrateAsync();
        Logger.Debug("DbMigration Completed");
      }      
    }
  }
}