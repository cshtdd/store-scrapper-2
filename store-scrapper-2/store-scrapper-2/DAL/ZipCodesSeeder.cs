using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using store_scrapper_2.DAL;
using store_scrapper_2.DAL.Db;

namespace store_scrapper_2
{
  public class ZipCodesSeeder
  {
    private readonly IStoreDataContextFactory _contextFactory;
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public ZipCodesSeeder(IStoreDataContextFactory contextFactory)
    {
      _contextFactory = contextFactory;
    }
    
    public async Task SeedAsync()
    {
      await ClearZipCodesAsync();
      await LoadZipCodesFromCsvAsync("zips.csv");
    }

    private async Task LoadZipCodesFromCsvAsync(string filename)
    {
      using (var context = _contextFactory.Create())
      {
        Logger.Debug("ZipCode Seed Started");

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