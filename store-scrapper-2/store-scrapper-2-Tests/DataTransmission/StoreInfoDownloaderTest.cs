using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;
using store_scrapper_2_Tests.Factory;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission
{
  public class StoreInfoDownloaderTest
  {
    [Fact]
    public async void DownloadsTheFirstStoreInfoFromTheStoreLocator()
    {
      var request = new StoreInfoRequest("77777-2");
      var seededResponse = StoresLocatorResponseFactory.Create("67789-4", "77785-1");

      var urlDownloader = Substitute.For<IUrlDownloader>();
      urlDownloader.DownloadAsync(request.ToUrl())
        .Returns(Task.FromResult(seededResponse));

      
      var responses = await new StoreInfoDownloader(urlDownloader)
        .DownloadAsync(request);
      var response = responses.First();

      
      response.StoreNumber.Should().Be(new StoreNumber("67789-4"));
      response.IsRestricted.Should().BeTrue();
      
      response.Address1.Should().Be("67789-4addr1");
      response.Address2.Should().Be("67789-4addr2");
      response.Address3.Should().Be("67789-4addr3");
      response.City.Should().Be("67789-4city1");
      response.CountryCode.Should().Be("67789-4us");
      response.CountryCode3.Should().Be("67789-4us3");
      response.PostalCode.Should().Be("67789-412345");
      response.State.Should().Be("67789-4ny");
      response.CurrentUtcOffset.Should().Be(5);
      
      response.Latitude.Should().Be(34);
      response.Longitude.Should().Be(67);
      response.TimeZoneId.Should().Be("67789-4GMT");

      response.CateringUrl.Should().Be("67789-4the catering");
      response.OrderingUrl.Should().Be("67789-4the ordering");
    }
  }
}