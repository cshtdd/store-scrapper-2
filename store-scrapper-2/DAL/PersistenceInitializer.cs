
using Microsoft.EntityFrameworkCore;
using store_scrapper_2.DAL;
using store_scrapper_2.Logging;

namespace store_scrapper_2
{
  public class PersistenceInitializer : IPersistenceInitializer
  {
    private readonly IStoreDataContextFactory _contextFactory;
    private readonly ZipCodesSeeder _zipCodesSeeder;
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public PersistenceInitializer(IStoreDataContextFactory contextFactory, ZipCodesSeeder zipCodesSeeder)
    {
      _contextFactory = contextFactory;
      _zipCodesSeeder = zipCodesSeeder;
    }
    
    public void Initialize()
    {
      RunMigrations();
      _zipCodesSeeder.Seed();
    }

    private void RunMigrations()
    {
      using (var context = _contextFactory.Create())
      {
        Logger.LogDebug("DbMigration Started");
        context.Database.Migrate();
        Logger.LogDebug("DbMigration Completed");
      }
    }
  }
}