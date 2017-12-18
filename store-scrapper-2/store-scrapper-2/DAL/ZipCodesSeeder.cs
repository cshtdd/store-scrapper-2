using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2.Configuration;
using store_scrapper_2.DAL;
using store_scrapper_2.DAL.Db;

namespace store_scrapper_2
{
  public class ZipCodesSeeder
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly IStoreDataContextFactory _contextFactory;
    private readonly IConfigurationReader _configurationReader;

    public ZipCodesSeeder(IStoreDataContextFactory contextFactory, IConfigurationReader configurationReader)
    {
      _contextFactory = contextFactory;
      _configurationReader = configurationReader;
    }
    
    public async Task SeedAsync()
    {
      Logger.Debug("ZipCode Seed Started");

      var shouldSeed = await ShouldSeedAsync();

      if (!shouldSeed)
      {
        Logger.Debug("ZipCode Seed Skipped");
        return;
      }
      
      await ClearZipCodesAsync();
      var seedFilename = _configurationReader.ReadString(ConfigurationKeys.SeedsZipsFilename);
      await LoadZipCodesFromCsvAsync(seedFilename);
    }

    private async Task<bool> ShouldSeedAsync()
    {
      using (var context = _contextFactory.Create())
      {
        var containsData = await context.Zips.AnyAsync();
        return !containsData;
      }
    }

    private async Task LoadZipCodesFromCsvAsync(string filename)
    {
      using (var context = _contextFactory.Create())
      {
        using (var streamReader = new StreamReader(filename))
        {
          var csvReader = new CsvReader(streamReader);
          csvReader.Configuration.HeaderValidated = null;
          csvReader.Configuration.MissingFieldFound = null;

          var zipCodes = csvReader.GetRecords<Zip>();
          await context.Zips.AddRangeAsync(zipCodes);
        }

        await context.SaveChangesAsync();

        Logger.Debug("ZipCode Seed Completed");
      }
    }
    
    private async Task ClearZipCodesAsync()
    {
      using (var context = _contextFactory.Create())
      {
        Logger.Debug("Clear ZipCodes Started");
        var allZipCodes = context.Zips.Select(_ => _).AsEnumerable();
        context.Zips.RemoveRange(allZipCodes);
        await context.SaveChangesAsync();
        Logger.Debug("Clear ZipCodes Completed");
      }
    }
    
  }
}