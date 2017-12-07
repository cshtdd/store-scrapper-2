using FluentAssertions;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission
{
  public class StoreInfoRequestTest
  {
    [Fact]
    public void GeneratesTheUrlBasedOnTheStoreNumber()
    {
      new StoreInfoRequest(new StoreNumber("32229-0"))
        .ToUrl()
        .Should()
        .Be(
          "https://locator-svc.subway.com/v2//GetLocations.ashx?&q=%7B%22InputText%22%3A%22%2332229-0%22%2C%22GeoCode%22%3A%7B%22Latitude%22%3A0%2C%22Longitude%22%3A0%2C%22Accuracy%22%3A0%2C%22CountryCode%22%3A%22%22%2C%22RegionCode%22%3Anull%2C%22PostalCode%22%3Anull%2C%22City%22%3Anull%2C%22LocalityType%22%3Anull%7D%2C%22DetectedLocation%22%3A%7B%22Latitude%22%3A0%2C%22Longitude%22%3A0%2C%22Accuracy%22%3A0%7D%2C%22Paging%22%3A%7B%22StartIndex%22%3A1%2C%22PageSize%22%3A10%7D%2C%22ConsumerParameters%22%3A%7B%22metric%22%3Afalse%2C%22culture%22%3A%22en-US%22%2C%22country%22%3A%22US%22%2C%22size%22%3A%22D%22%2C%22template%22%3A%22%22%2C%22rtl%22%3Afalse%2C%22clientId%22%3A%2217%22%2C%22key%22%3A%22SUBWAY_PROD%22%7D%2C%22Filters%22%3A%5B%5D%2C%22LocationType%22%3A4%7D"
        );
    }

    [Fact]
    public void CalculatesTheFullStoreNumber()
    {
      new StoreInfoRequest(new StoreNumber("11111", "2"))
        .ToString()
        .Should()
        .Be("StoreInfoRequest 11111-2");
    }
  }
}