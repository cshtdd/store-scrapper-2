using store_scrapper_2.DataTransmission;

namespace store_scrapper_2_int_Tests.Utils
{
  public static class StoreInfoResponseFactory
  {
    public static StoreInfoResponse Create(string fullStoreNumber)
    {
      var storeNumber = fullStoreNumber.Split('-')[0];
      var satelliteNumber = fullStoreNumber.Split('-')[1];

      return new StoreInfoResponse
      {
        StoreNumber = storeNumber,
        SatelliteNumber = satelliteNumber,
        Address1 = $"{fullStoreNumber}addr1",
        Address2 = $"{fullStoreNumber}addr2",
        Address3 = $"{fullStoreNumber}addr3",
        CateringUrl = $"{fullStoreNumber}the catering",
        City = $"{fullStoreNumber}the city",
        CountryCode = $"{fullStoreNumber}us",
        CountryCode3 = $"{fullStoreNumber}usa",
        CurrentUtcOffset = 5,
        IsRestricted = true,
        Latitude = 12,
        Longitude = 16,
        ListingNumber = 3,
        OrderingUrl = $"{fullStoreNumber}the ordering",
        PostalCode = $"{fullStoreNumber}12345",
        State = $"{fullStoreNumber}fl",
        TimeZoneId = $"{fullStoreNumber}EST"
      };
    }
  }
}