using System.Linq;
using FluentAssertions;
using store_scrapper_2.DataTransmission;
using Xunit;

namespace store_screapper_2_Tests.DataTransmission
{
  public class StoreInfoResposeTest
  {
    [Fact]
    public void CanBeCreatedOutOfAString()
    {
      var json = @"
      ({
          ""ResultData"": [
            {
              ""LocationId"":
              {
                ""StoreNumber"": 11111,
                ""SatelliteNumber"": 0,
                ""IsRestricted"": true
              },
              ""Address"":
              {
                ""Address1"": ""360 Lyndock St"",
                ""Address2"": ""Box 653"",
                ""Address3"": ""third address"",
                ""City"": ""Corunna"",
                ""StateProvCode"": ""ON"",
                ""PostalCode"": ""N0N 1G0"",
                ""CountryCode"": ""CA"",
                ""CountryCode3"": ""CAN""
              },
              ""Geo"":
              {
                ""Latitude"": 42.8915,
                ""Longitude"": -82.4534,
                ""TimeZoneId"": ""America/Detroit"",
                ""CurrentUtcOffset"": 1
              },
              ""ListingNumber"": 1,
              ""OrderingUrl"": ""http://order.subway.com/Stores/Redirect.aspx?s=11111&sa=0&f=r"",
              ""CateringUrl"": ""http://order.subway.com/Stores/Redirect.aspx?s=11111&sa=0&f=c""
            }],
            ""ResultHtml"": ["""", ""\r\n\r\n<div class=\""location \"" id=\""locationId_11111-0\"" >""],
            ""PagingHtml"": ""\r\n<div class=\""pagingHolder\"">\r\n"",
            ""ResultsHeadingHtml"": null
      })".Split("\n")
        .Select(_ => _.TrimEnd())
        .Select(_ => _.TrimStart())
        .Aggregate((a, b) => a + b);

      var response = StoreInfoResponse.Parse(json);
      
      response.FullStoreNumber.Should().Be("11111-0");
      response.IsRestricted.Should().Be(true);

      response.Address1.Should().Be("360 Lyndock St");
      response.Address2.Should().Be("Box 653");
      response.Address3.Should().Be("third address");
      response.City.Should().Be("Corunna");
      response.State.Should().Be("ON");
      response.PostalCode.Should().Be("N0N 1G0");
      response.CountryCode.Should().Be("CA");
      response.CountryCode3.Should().Be("CAN");

      response.Latitude.Should().Be(42.8915);
      response.Longitude.Should().Be(-82.4534);
      response.TimeZoneId.Should().Be("America/Detroit");
      response.CurrentUtcOffset.Should().Be(1);

      response.ListingNumber.Should().Be(1);
      response.OrderingUrl.Should().Be("http://order.subway.com/Stores/Redirect.aspx?s=11111&sa=0&f=r");
      response.CateringUrl.Should().Be("http://order.subway.com/Stores/Redirect.aspx?s=11111&sa=0&f=c");
    }
  }
}