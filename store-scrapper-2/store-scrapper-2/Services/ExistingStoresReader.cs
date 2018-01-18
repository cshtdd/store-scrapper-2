using System;
using System.Threading.Tasks;
using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public class ExistingStoresReader : IExistingStoresReader
  {
    private readonly IStoreInfoResponseDataService _dataService;
    private volatile bool _isInitialized;

    public ExistingStoresReader(IStoreInfoResponseDataService dataService) => _dataService = dataService;

    public async Task InitializeAsync()
    {
      if (_isInitialized)
      {
        throw new InvalidOperationException($"ExistingStoresInitialization; reason=AlreadyInitialized;");
      }
      
      await _dataService.AllStoreNumbersAsync();
      _isInitialized = true;
    }

    public bool ContainsStores(StoreNumber storeNumber)
    {
      throw new System.NotImplementedException();
    }
  }
}