using System.Threading.Tasks;
using Castle.DynamicProxy.Generators;
using FluentAssertions;
using NSubstitute;
using store;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DataTransmission.Serialization;
using Xunit;

namespace store_screapper_2_Tests.DataTransmission
{
  public class StoreInfoDownloaderTest
  {
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
              Address2 = "addr2",
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
      return $"({seededStoreLocatorResponse.ToJson()})";
    }

    [Fact]
    public async void DownloadsTheStoreInfoFromTheStoreLocator()
    {
      var request = new StoreInfoRequest("77777", "2");

      var urlDownloader = Substitute.For<IUrlDownloader>();
      urlDownloader.DownloadAsync(request.ToUrl())
        .Returns(Task.FromResult(GenerateStoreLocatorResponse()));

      
      var response = await new StoreInfoDownloader(urlDownloader)
        .DownloadAsync(request);

      
      response.FullStoreNumber.Should().Be("67789-4");
      response.IsRestricted.Should().BeTrue();
      
      response.Address1.Should().Be("addr1");
      response.Address2.Should().Be("addr2");
      response.Address3.Should().Be("addr3");
      response.City.Should().Be("city1");
      response.CountryCode.Should().Be("us");
      response.CountryCode3.Should().Be("us3");
      response.PostalCode.Should().Be("12345");
      response.State.Should().Be("ny");
      response.CurrentUtcOffset.Should().Be(5);
      
      response.Latitude.Should().Be(34);
      response.Longitude.Should().Be(67);
      response.TimeZoneId.Should().Be("GMT");

      response.ListingNumber.Should().Be(12);
      response.CateringUrl.Should().Be("the catering");
      response.OrderingUrl.Should().Be("the ordering");
    }
  }
}