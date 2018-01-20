using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2.Services
{
  public interface IStoresPersistor
  {
    [Obsolete]
    Task PersistAsync(StoreInfo storeInfo);

    Task PersistAsync(IEnumerable<StoreInfo> stores);
  }
}