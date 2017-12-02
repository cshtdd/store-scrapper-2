using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using store_scrapper_2;
using store_scrapper_2.DataTransmission;
using store_scrapper_2_int_Tests.Utils;
using Xunit;

namespace store_scrapper_2_int_Tests.DAL
{
  public class StoreInfoResponseDataServiceTest : BaseDatabaseTest
  {   
    [Fact]
    public async Task SavesANewResponse()
    {
      await new PersistenceInitializer(ContextFactory).InitializeAsync();
      
      var response = new StoreInfoResponse
      {
        StoreNumber = "11111",
        SatelliteNumber = "3",
        Address1 = "addr1",
        Address2 = "addr2",
        Address3 = "addr3",
        CateringUrl = "the catering",
        City = "the city",
        CountryCode = "us",
        CountryCode3 = "usa",
        CurrentUtcOffset = 5,
        IsRestricted = true,
        Latitude = 12,
        Longitude = 16,
        ListingNumber = 3,
        OrderingUrl = "the ordering",
        PostalCode = "12345",
        State = "fl",
        TimeZoneId = "EST"
      };

      using (var context = ContextFactory.Create())
      {
        context.Stores.Should().BeEmpty();
      }
      
      await new StoreInfoResponseDataService(ContextFactory).CreateNewAsync(response);
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Should().NotBeEmpty();
      }
    }
  }
}