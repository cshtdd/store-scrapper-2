using System;
using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DAL;

namespace store
{
  public class StoreInfoResponseDataService : IStoreInfoResponseDataService
  {
    public async Task SaveAsync(StoreInfoResponse response)
    {
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

        if (changedEntries == 0)
        {
          throw new InvalidOperationException();
        }
      }
    }
  }
}