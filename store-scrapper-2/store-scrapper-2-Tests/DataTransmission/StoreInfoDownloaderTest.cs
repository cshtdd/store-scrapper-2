using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2.Model;
using store_scrapper_2.DataTransmission;
using store_scrapper_2_Tests.Factory;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission
{
  public class StoreInfoDownloaderTest
  {
    [Fact]
    public async void DownloadsTheFirstStoreInfoFromTheStoreLocator()
    {
      var zipCode = new ZipCode("33009", 12.23m, 45.67m);
      var seededResponse = StoresLocatorResponseFactory.Create("67789-4", "77785-1");

      var urlSerializer = Substitute.For<IZipCodeUrlSerializer>();
      urlSerializer.ToUrl(zipCode)
        .Returns("the converted url");
      
      var urlDownloader = Substitute.For<IUrlDownloader>();
      urlDownloader.DownloadAsync("the converted url")
        .Returns(Task.FromResult(seededResponse));


      var responses = (await new StoreInfoDownloader(urlDownloader, urlSerializer)
        .DownloadAsync(zipCode))
        .ToList();

      responses.Count.Should().Be(2);
    
      responses[0].StoreNumber.Should().Be(new StoreNumber("67789-4"));
      responses[0].IsRestricted.Should().BeTrue();
      responses[0].Address1.Should().Be("67789-4addr1");
      responses[0].Address2.Should().Be("67789-4addr2");
      responses[0].Address3.Should().Be("67789-4addr3");
      responses[0].City.Should().Be("67789-4city1");
      responses[0].CountryCode.Should().Be("67789-4us");
      responses[0].CountryCode3.Should().Be("67789-4us3");
      responses[0].PostalCode.Should().Be("67789-412345");
      responses[0].State.Should().Be("67789-4ny");
      responses[0].CurrentUtcOffset.Should().Be(5);
      responses[0].Latitude.Should().Be(34);
      responses[0].Longitude.Should().Be(67);
      responses[0].TimeZoneId.Should().Be("67789-4GMT");
      responses[0].CateringUrl.Should().Be("67789-4the catering");
      responses[0].OrderingUrl.Should().Be("67789-4the ordering");
      
      responses[1].StoreNumber.Should().Be(new StoreNumber("77785-1"));
      responses[1].IsRestricted.Should().BeTrue();
      responses[1].Address1.Should().Be("77785-1addr1");
      responses[1].Address2.Should().Be("77785-1addr2");
      responses[1].Address3.Should().Be("77785-1addr3");
      responses[1].City.Should().Be("77785-1city1");
      responses[1].CountryCode.Should().Be("77785-1us");
      responses[1].CountryCode3.Should().Be("77785-1us3");
      responses[1].PostalCode.Should().Be("77785-112345");
      responses[1].State.Should().Be("77785-1ny");
      responses[1].CurrentUtcOffset.Should().Be(5);
      responses[1].Latitude.Should().Be(34);
      responses[1].Longitude.Should().Be(67);
      responses[1].TimeZoneId.Should().Be("77785-1GMT");
      responses[1].CateringUrl.Should().Be("77785-1the catering");
      responses[1].OrderingUrl.Should().Be("77785-1the ordering");
    }
  }
}