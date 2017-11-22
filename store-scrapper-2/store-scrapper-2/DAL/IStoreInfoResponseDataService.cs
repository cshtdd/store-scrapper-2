using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;

namespace store
{
  public interface IStoreInfoResponseDataService
  {
    Task<bool> ContainsStoreAsync(string storeNumber, string satellite);
    Task CreateNewAsync(StoreInfoResponse response);
    Task UpdateAsync(StoreInfoResponse response);
  }
}