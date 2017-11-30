using System.Linq;
using store_scrapper_2.DataTransmission.Serialization;

namespace store_scrapper_2_Tests.Factory
{
  public static class StoresLocatorResponseFactory
  {
    public static string Create(params string[] storeNumbers)
    {
      var seededStoreLocatorResponse = new StoresLocatorResponse
      {
        ResultData = storeNumbers.Select(StoreInfoDataFactory.Create).ToArray()
      };
      return $"({seededStoreLocatorResponse.ToJson()})";
    }
  }
}