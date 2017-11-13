using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DAL;

namespace store_scrapper_2
{
  static class Program
  {
    // ReSharper disable once UnusedParameter.Local
    public static async Task Main(string[] args)
    {
      Console.WriteLine($"Launching Program with args={string.Join(",", args)}");
      
      var response = await DownloadStoreData();
      Console.WriteLine($"response={response}");

      SaveStoreData(response);
    }

    private static async void SaveStoreData(StoreInfoResponse response)
    {
      Console.WriteLine(new string('=', 16));

      using (var db = new StoreDataContext())
      {
        db.Stores.Add(new Store
        {
          StoreNumber = response.StoreNumber,
          SatelliteNumber = response.SatelliteNumber,
          IsRestricted = response.IsRestricted,
          Address1 = response.Address1,
          Address2 = response.Address2,
          Address3 = response.Address3,
          City = response.City,
          State = response.State,
          PostalCode = response.PostalCode,
          CountryCode = response.CountryCode,
          CountryCode3 = response.CountryCode3,
          Latitude = response.Latitude,
          Longitude = response.Longitude,
          TimeZoneId = response.TimeZoneId,
          CurrentUtcOffset = response.CurrentUtcOffset,
          ListingNumber = response.ListingNumber,
          OrderingUrl = response.OrderingUrl,
          CateringUrl = response.CateringUrl
        });
        var changedEntries = await db.SaveChangesAsync();
        
        Console.WriteLine($"changedEntries={changedEntries}");
      }

      Console.WriteLine(new string('=', 16));
    }

    private static async Task<StoreInfoResponse> DownloadStoreData()
    {
      var request = new StoreInfoRequest("11111", "0");

      Console.WriteLine($"Sending Request: {request}");

      return await CreateDownloader().DownloadAsync(request);
    }

    private static StoreInfoDownloader CreateDownloader() => new StoreInfoDownloader(new UrlDownloader());
  }
}
