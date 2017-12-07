using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;

namespace store_scrapper_2
{
  public interface IStoreInfoResponseDataService
  {
    Task<bool> ContainsStoreAsync(StoreNumber storeNumber);
    Task CreateNewAsync(StoreInfoResponse response);
    Task UpdateAsync(StoreInfoResponse response);
  }
}