using System.IO;
using System.Linq;

using CsvHelper;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2.Configuration;
using store_scrapper_2.DAL;
using store_scrapper_2.DAL.Db;
using store_scrapper_2.Logging;

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
    
    public void Seed()
    {
      Logger.LogDebug("ZipCode Seed Started");

      var shouldSeed = ShouldSeed();

      if (!shouldSeed)
      {
        Logger.LogDebug("ZipCode Seed Skipped");
        return;
      }
      
      ClearZipCodes();
      var seedFilename = _configurationReader.ReadString(ConfigurationKeys.SeedsZipsFilename);
      LoadZipCodesFromCsv(seedFilename);
    }

    private bool ShouldSeed()
    {
      using (var context = _contextFactory.Create())
      {
        var containsData = context.Zips.Any();
        return !containsData;
      }
    }

    private void LoadZipCodesFromCsv(string filename)
    {
      using (var context = _contextFactory.Create())
      {
        using (var streamReader = new StreamReader(filename))
        {
          var csvReader = new CsvReader(streamReader);
          csvReader.Configuration.HeaderValidated = null;
          csvReader.Configuration.MissingFieldFound = null;

          var zipCodes = csvReader.GetRecords<Zip>();
          context.Zips.AddRange(zipCodes);
        }

        context.SaveChanges();

        Logger.LogDebug("ZipCode Seed Completed");
      }
    }
    
    private void ClearZipCodes()
    {
      using (var context = _contextFactory.Create())
      {
        Logger.LogDebug("Clear ZipCodes Started");
        var allZipCodes = context.Zips.Select(_ => _).AsEnumerable();
        context.Zips.RemoveRange(allZipCodes);
        context.SaveChanges();
        Logger.LogDebug("Clear ZipCodes Completed");
      }
    }
    
  }
}