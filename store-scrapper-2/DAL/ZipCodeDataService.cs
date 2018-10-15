using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using store_scrapper_2.Model;
using store_scrapper_2.DAL;
using store_scrapper_2.DAL.Db;
using store_scrapper_2.Logging;

namespace store_scrapper_2
{
  public class ZipCodeDataService : IZipCodeDataService
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly IStoreDataContextFactory _contextFactory;

    public ZipCodeDataService(IStoreDataContextFactory contextFactory) => _contextFactory = contextFactory;

    public IEnumerable<ZipCodeInfo> All()
    {
      using (var db = _contextFactory.Create())
      {
        Logger.LogDebug("All;");

        return db.Zips.Select(_ => new ZipCodeInfo
        {
          ZipCode = _.ToZipCode(),
          UpdateTimeUtc = _.UpdateTimeUtc ?? DateTime.MinValue
        }).ToArray();
      }
    }

    public void UpdateZipCode(string zipCode)
    {
      using (var db = _contextFactory.Create())
      {
        Logger.LogDebug("UpdateZipCode", nameof(zipCode), zipCode);

        var zip = db.Zips.First(_ => _.ZipCode == zipCode);
        zip.UpdateTimeUtc = DateTime.UtcNow;
        db.SaveChanges();
      }
    }
  }

  internal static class ZipExtensions
  {
    public static ZipCode ToZipCode(this Zip zip) => new ZipCode(zip.ZipCode, zip.Latitude, zip.Longitude);
  }
}