using System.Linq;
using store_scrapper_2.DataTransmission.Serialization;
using Xunit;

namespace store_screapper_2_Tests.DataTransmission.Serialization
{
  public class StoreLocatorQueryTest
  {
    [Fact]
    public void CanBeConvertedToJson()
    {
      var expected = @"{
        ""InputText"": ""AAAA"",

        ""GeoCode"": {
          ""Latitude"": 0,
          ""Longitude"": 0,
          ""Accuracy"": 0,
          ""CountryCode"": """",
          ""RegionCode"": null,
          ""PostalCode"": null,
          ""City"": null,
          ""LocalityType"": null
        },

        ""DetectedLocation"":
        {
          ""Latitude"": 0,
          ""Longitude"": 0,
          ""Accuracy"": 0
        },

        ""Paging"":
        {
          ""StartIndex"": 1,
          ""PageSize"": 10
        },

        ""ConsumerParameters"":
        {
          ""metric"": false,
          ""culture"": ""en-US"",
          ""country"": ""US"",
          ""size"": ""D"",
          ""template"": """",
          ""rtl"": false,
          ""clientId"": ""11"",
          ""key"": ""chipotle""
        },

        ""Filters"": [],
        ""LocationType"": 3
      }".Split()
        .Select(_ => _.Trim())
        .Aggregate((a, b) => a + b);  

      var actual = new StoreLocatorQuery("AAAA", 3, "11", "chipotle").ToJson();
      
      Assert.Equal(expected, actual);
    }
  }
}