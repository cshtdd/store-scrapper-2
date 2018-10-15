using System.Collections.Generic;

using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;

namespace store_scrapper_2
{
  public interface IStoreInfoResponseDataService
  {
    IEnumerable<StoreNumber> ContainsStore(IEnumerable<StoreNumber> storeNumber);
    void CreateNew(IEnumerable<StoreInfo> storeInfo);
    void Update(IEnumerable<StoreInfo> storeInfo);
  }
}