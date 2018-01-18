using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public class ExistingStoresReader : IExistingStoresReader
  {
    private readonly IStoreInfoResponseDataService _dataService;
    private volatile bool _isInitialized;
    private readonly HashSet<string> _storeNumbersCache = new HashSet<string>();

    public ExistingStoresReader(IStoreInfoResponseDataService dataService) => _dataService = dataService;

    public async Task InitializeAsync()
    {
      if (_isInitialized)
      {
        throw new InvalidOperationException("ExistingStoresInitialization; reason=AlreadyInitialized;");
      }

      foreach (var storeNumber in await _dataService.AllStoreNumbersAsync())
      {
        _storeNumbersCache.Add(storeNumber.ToString());
      }
      _isInitialized = true;
    }

    public bool ContainsStores(StoreNumber storeNumber)
    {
      if (!_isInitialized)
      {
        throw new InvalidOperationException("ContainsStore; reason=NotInitialized;");       
      }

      return _storeNumbersCache.Contains(storeNumber.ToString());
    }
  }
}