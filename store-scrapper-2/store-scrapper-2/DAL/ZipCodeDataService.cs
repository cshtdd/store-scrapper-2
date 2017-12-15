using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2.DataTransmission;
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
  }

  internal static class ZipExtensions
  {
    public static ZipCode ToZipCode(this Zip zip) => new ZipCode(zip.ZipCode, zip.Latitude, zip.Longitude);
  }
}