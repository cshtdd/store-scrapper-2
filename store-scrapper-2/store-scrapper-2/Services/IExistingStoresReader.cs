using System.Threading.Tasks;
using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public interface IExistingStoresReader
  {
    Task InitializeAsync();
    bool ContainsStore(StoreNumber storeNumber);
  }
}