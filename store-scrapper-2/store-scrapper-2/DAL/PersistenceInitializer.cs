using System.Threading.Tasks;
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
    
    public async Task InitializeAsync()
    {
      await RunMigrationsAsync();
      await _zipCodesSeeder.SeedAsync();
    }

    private async Task RunMigrationsAsync()
    {
      using (var context = _contextFactory.Create())
      {
        Logger.LogDebug("DbMigration Started");
        await context.Database.MigrateAsync();
        Logger.LogDebug("DbMigration Completed");
      }
    }
  }
}