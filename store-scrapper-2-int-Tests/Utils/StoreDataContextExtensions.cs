using System.Linq;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DAL.Db;
using store_scrapper_2.Model;

namespace store_scrapper_2_int_Tests.Utils
{
  public static class StoreDataContextExtensions
  {
    public static void ShouldContainStoreEquivalentTo(this StoreDataContext context, StoreInfo response)
    {
      var dbStore = context.Stores.First(_ => response.StoreNumber == new StoreNumber(_.StoreNumber, _.SatelliteNumber));
      dbStore.ShouldBeEquivalentTo(response);
    }
  }
}