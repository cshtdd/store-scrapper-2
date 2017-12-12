using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using store_scrapper_2.DAL;
using store_scrapper_2.DAL.Db;

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
      await RunMigrationsAsync();
      await SeedZipCodesAsync();
    }

    private async Task RunMigrationsAsync()
    {
      using (var context = _contextFactory.Create())
      {
        Logger.Debug("DbMigration Started");
        await context.Database.MigrateAsync();
        Logger.Debug("DbMigration Completed");
      }
    }

    private async Task SeedZipCodesAsync()
    {
      using (var context = _contextFactory.Create())
      {
        Logger.Debug("ZipCode Seed Started");

//        var truncateCommand = $"TRUNCATE TABLE public.\"{nameof(context.Zips)}\"";
//        await context.Database.ExecuteSqlCommandAsync(truncateCommand);

        var allZipCodes = context.Zips.Select(_ => _).AsEnumerable();
        context.Zips.RemoveRange(allZipCodes);

        using (var streamReader = new StreamReader("zips.csv"))
        {
          var csvReader = new CsvReader(streamReader);
          csvReader.Configuration.HeaderValidated = null;
          csvReader.Configuration.MissingFieldFound = null;

          await context.Zips.AddRangeAsync(csvReader.GetRecords<Zip>());
        }

        await context.SaveChangesAsync();
        
        Logger.Debug("ZipCode Seed Completed");
      }
    }
  }
}