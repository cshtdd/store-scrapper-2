using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;

namespace store_scrapper_2
{
  public interface IStoreInfoResponseDataService
  {
    Task<IEnumerable<StoreNumber>> ContainsStoreAsync(IEnumerable<StoreNumber> storeNumber);
    Task CreateNewAsync(IEnumerable<StoreInfo> storeInfo);
    Task UpdateAsync(IEnumerable<StoreInfo> storeInfo);
    
    [Obsolete]
    Task<bool> ContainsStoreAsync(StoreNumber storeNumber);   
    [Obsolete]
    Task CreateNewAsync(StoreInfo storeInfo);
    [Obsolete]
    Task UpdateAsync(StoreInfo storeInfo);
  }
}