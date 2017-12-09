using System.Linq;
using FluentAssertions;
using store_scrapper_2.DataTransmission.Serialization;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission.Serialization
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
          ""Accuracy"": null,
          ""CountryCode"": null,
          ""RegionCode"": null,
          ""PostalCode"": ""33123"",
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
          ""PageSize"": 30
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

      new StoreLocatorQuery("AAAA", "33123", 3, "11", "chipotle", 30)
        .ToJson()
        .Should()
        .Be(expected);
    }
  }
}