
using store_scrapper_2;
using store_scrapper_2.Model;

namespace store_scrapper_2_Tests.Factory
{
  public static class StoreInfoDataFactory
  {
    public static StoreInfoData Create(StoreNumber storeNumber)
    {
      return new StoreInfoData
      {
        LocationId = new LocationId
        {
          StoreNumber = storeNumber.Store,
          SatelliteNumber = storeNumber.Satellite,
          IsRestricted = true
        },
        Address = new Address
        {
          Address1 = $"{storeNumber}addr1",
          Address2 = $"{storeNumber}addr2",
          Address3 = $"{storeNumber}addr3",
          City = $"{storeNumber}city1",
          CountryCode = $"{storeNumber}us",
          CountryCode3 = $"{storeNumber}us3",
          PostalCode = $"{storeNumber}12345",
          StateProvCode = $"{storeNumber}ny"
        },
        Geo = new Geo
        {
          CurrentUtcOffset = 5,
          Latitude = 34,
          Longitude = 67,
          TimeZoneId = $"{storeNumber}GMT"
        },
        ListingNumber = 12,
        CateringUrl = $"{storeNumber}the catering",
        OrderingUrl = $"{storeNumber}the ordering"
      };
    }
  }
}