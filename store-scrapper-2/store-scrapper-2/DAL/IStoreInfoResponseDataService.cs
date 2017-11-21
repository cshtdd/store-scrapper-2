using System;
using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;

namespace store
{
  public interface IStoreInfoResponseDataService
  {
    Task SaveAsync(StoreInfoResponse response);
  }
}