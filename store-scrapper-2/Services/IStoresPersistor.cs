using System.Collections.Generic;

using store_scrapper_2.DataTransmission;

namespace store_scrapper_2.Services
{
  public interface IStoresPersistor
  {
    void Persist(IEnumerable<StoreInfo> stores);
  }
}