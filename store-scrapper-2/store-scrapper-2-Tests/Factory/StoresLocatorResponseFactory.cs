using System.Linq;
using store_scrapper_2.DataTransmission.Serialization;
using store_scrapper_2.Model;

namespace store_scrapper_2_Tests.Factory
{
  public static class StoresLocatorResponseFactory
  {
    public static string Create(params StoreNumber[] storeNumbers)
    {
      var seededStoreLocatorResponse = new StoresLocatorResponse
      {
        ResultData = storeNumbers.Select(StoreInfoDataFactory.Create).ToArray()
      };
      return $"({seededStoreLocatorResponse.ToJson()})";
    }
  }
}