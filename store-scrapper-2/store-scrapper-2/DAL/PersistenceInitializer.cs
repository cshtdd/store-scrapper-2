using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2.DAL;

namespace store_scrapper_2
{
  public class PersistenceInitializer : IPersistenceInitializer
  {
    private readonly IStoreDataContextFactory _contextFactory;
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public PersistenceInitializer(IStoreDataContextFactory contextFactory)
    {
      _contextFactory = contextFactory;
    }
    
    public async Task InitializeAsync()
    {
      using (var context = _contextFactory.Create())
      {
        Logger.Debug("DbMigration Started");
        await context.Database.MigrateAsync();
        Logger.Debug("DbMigration Completed");
      }      
    }
  }
}