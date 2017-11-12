using store;
using store_scrapper_2.DataTransmission.Serialization;
using Xunit;

namespace store_screapper_2_Tests.DataTransmission
{
  public class StoreInfoDownloaderTest
  {
    [Fact]
    public void DownloadsTheStoreInfoFromTheStoreLocator()
    {
      var seededResponse = GenerateStoreLocatorResponse();
      
    }

    private static string GenerateStoreLocatorResponse()
    {
      var seededStoreLocatorResponse = new StoresLocatorResponse
      {
        ResultData = new[]
        {
          new StoreInfoData
          {
            LocationId = new LocationId
            {
              StoreNumber = 67789,
              SatelliteNumber = 4,
              IsRestricted = true
            },
            Address = new Address
            {
              Address1 = "addr1",
              Address2 = "add2",
              Address3 = "addr3",
              City = "city1",
              CountryCode = "us",
              CountryCode3 = "us3",
              PostalCode = "12345",
              StateProvCode = "ny"
            },
            Geo = new Geo
            {
              CurrentUtcOffset = 5,
              Latitude = 34,
              Longitude = 67,
              TimeZoneId = "GMT"
            },
            ListingNumber = 12,
            CateringUrl = "the catering",
            OrderingUrl = "the ordering"
          }
        }
      };
      var seededResponse = $"({seededStoreLocatorResponse.ToJson()})";
      return seededResponse;
    }
  }
}