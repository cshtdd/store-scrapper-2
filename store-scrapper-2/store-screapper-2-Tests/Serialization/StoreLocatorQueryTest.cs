﻿using System;
using System.Linq;
using store_scrapper_2.Serialization;
using Xunit;

namespace store_screapper_2_Tests.Serialization
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

        ""Filters"": [],
        ""LocationType"": 3
      }".Split()
        .Select(_ => _.Trim())
        .Aggregate((a, b) => a + b);  

      var actual = new StoreLocatorQuery("AAAA", 3).ToString();
      
      Assert.Equal(expected, actual);
    }
  }
}