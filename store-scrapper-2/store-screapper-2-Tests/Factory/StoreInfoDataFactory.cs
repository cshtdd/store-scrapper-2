using store;

namespace store_screapper_2_Tests.Factory
{
  public static class StoreInfoDataFactory
  {
    public static StoreInfoData Create(string fullStoreNumber)
    {
      var storeNumber = int.Parse(fullStoreNumber.Split('-')[0]);
      var satelliteNumber = int.Parse(fullStoreNumber.Split('-')[1]);

      return new StoreInfoData
      {
        LocationId = new LocationId
        {
          StoreNumber = storeNumber,
          SatelliteNumber = satelliteNumber,
          IsRestricted = true
        },
        Address = new Address
        {
          Address1 = $"{fullStoreNumber}addr1",
          Address2 = $"{fullStoreNumber}addr2",
          Address3 = $"{fullStoreNumber}addr3",
          City = $"{fullStoreNumber}city1",
          CountryCode = $"{fullStoreNumber}us",
          CountryCode3 = $"{fullStoreNumber}us3",
          PostalCode = $"{fullStoreNumber}12345",
          StateProvCode = $"{fullStoreNumber}ny"
        },
        Geo = new Geo
        {
          CurrentUtcOffset = 5,
          Latitude = 34,
          Longitude = 67,
          TimeZoneId = $"{fullStoreNumber}GMT"
        },
        ListingNumber = 12,
        CateringUrl = $"{fullStoreNumber}the catering",
        OrderingUrl = $"{fullStoreNumber}the ordering"
      };
    }

  }
}