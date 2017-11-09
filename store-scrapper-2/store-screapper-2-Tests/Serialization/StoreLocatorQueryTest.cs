using System;
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
        ""Filters"": [],
        ""LocationType"": 3
      }".Split()
        .Select(_ => _.Trim())
        .Aggregate((a, b) => a + b);  

      var actual = new StoreLocatorQuery
      {
        InputText = "AAAA",
        LocationType = 3
      }.ToString();
      
      Assert.Equal(expected, actual);
    }
  }
}