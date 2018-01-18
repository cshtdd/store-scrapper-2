using System.Threading.Tasks;
using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public class ExistingStoresReader : IExistingStoresReader
  {
    private readonly IStoreInfoResponseDataService _dataService;

    public ExistingStoresReader(IStoreInfoResponseDataService dataService) => _dataService = dataService;

    public async Task InitializeAsync()
    {
      await _dataService.AllStoreNumbersAsync();
    }

    public bool ContainsStores(StoreNumber storeNumber)
    {
      throw new System.NotImplementedException();
    }
  }
}