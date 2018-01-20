using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;

namespace store_scrapper_2_Tests.Factory
{
  public static class StoreInfoFactory
  {
    public static StoreInfo Create(StoreNumber storeNumber) => new StoreInfo
    {
      StoreNumber = storeNumber,
      Address1 = $"{storeNumber}addr1",
      Address2 = $"{storeNumber}addr2",
      Address3 = $"{storeNumber}addr3",
      CateringUrl = $"{storeNumber}the catering",
      City = $"{storeNumber}the city",
      CountryCode = $"{storeNumber}us",
      CountryCode3 = $"{storeNumber}usa",
      CurrentUtcOffset = 5,
      IsRestricted = true,
      Latitude = 12,
      Longitude = 16,
      OrderingUrl = $"{storeNumber}the ordering",
      PostalCode = $"{storeNumber}12345",
      State = $"{storeNumber}fl",
      TimeZoneId = $"{storeNumber}EST"
    };
  }
}