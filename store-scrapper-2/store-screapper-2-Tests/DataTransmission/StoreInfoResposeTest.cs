using System.Linq;
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
      
      Assert.Equal("11111-0", response.FullStoreNumber);
      Assert.Equal(true, response.IsRestricted);
      
      Assert.Equal("360 Lyndock St", response.Address1);
      Assert.Equal("Box 653", response.Address2);
      Assert.Equal("third address", response.Address3);
      Assert.Equal("Corunna", response.City);
      Assert.Equal("ON", response.State);
      Assert.Equal("N0N 1G0", response.PostalCode);
      Assert.Equal("CA", response.CountryCode);
      Assert.Equal("CAN", response.CountryCode3);
      
      Assert.Equal(42.8915, response.Latitude);
      Assert.Equal(-82.4534, response.Longitude);
      Assert.Equal("America/Detroit", response.TimeZoneId);
      Assert.Equal(1, response.CurrentUtcOffset);
      
      Assert.Equal(1, response.ListingNumber);
      Assert.Equal("http://order.subway.com/Stores/Redirect.aspx?s=11111&sa=0&f=r", response.OrderingUrl);
      Assert.Equal("http://order.subway.com/Stores/Redirect.aspx?s=11111&sa=0&f=c", response.CateringUrl);
    }
  }
}