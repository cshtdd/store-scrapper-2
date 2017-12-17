using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2.Model;
using store_scrapper_2.DAL;
using store_scrapper_2.DAL.Db;

namespace store_scrapper_2
{
  public class ZipCodeDataService : IZipCodeDataService
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly IStoreDataContextFactory _contextFactory;

    public ZipCodeDataService(IStoreDataContextFactory contextFactory)
    {
      _contextFactory = contextFactory;
    }

    public async Task<ZipCode> ReadAsync(string zipCode)
    {
      using (var db = _contextFactory.Create())
      {
        Logger.Debug($"ReadAsync; {nameof(zipCode)}={zipCode}");

        var zip = await db.Zips.FirstAsync(_ => _.ZipCode == zipCode);
        return zip.ToZipCode();
      }
    }

    public async Task<IEnumerable<ZipCodeInfo>> AllAsync()
    {
      using (var db = _contextFactory.Create())
      {
        Logger.Debug("AllAsync;");

        return await db.Zips.Select(_ => new ZipCodeInfo
        {
          ZipCode = _.ToZipCode(),
          UpdateTimeUtc = _.UpdateTimeUtc ?? DateTime.MinValue
        }).ToArrayAsync();
      }
    }

    public async Task UpdateZipCodeAsync(string zipCode)
    {
      using (var db = _contextFactory.Create())
      {
        Logger.Debug($"UpdateZipCodeAsync; {nameof(zipCode)}={zipCode}");

        var zip = db.Zips.First(_ => _.ZipCode == zipCode);
        zip.UpdateTimeUtc = DateTime.UtcNow;
        await db.SaveChangesAsync();
      }
    }
  }

  internal static class ZipExtensions
  {
    public static ZipCode ToZipCode(this Zip zip) => new ZipCode(zip.ZipCode, zip.Latitude, zip.Longitude);
  }
}