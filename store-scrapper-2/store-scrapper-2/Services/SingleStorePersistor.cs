﻿using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2.Services
{
  public class SingleStorePersistor : ISingleStorePersistor
  {
    private readonly IStoreInfoResponseDataService _dataService;
    private readonly IStorePersistenceCalculator _persistenceCalculator;
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public SingleStorePersistor(IStoreInfoResponseDataService dataService,
      IStorePersistenceCalculator persistenceCalculator)
    {
      _dataService = dataService;
      _persistenceCalculator = persistenceCalculator;
    }

    public async Task PersistAsync(StoreInfo storeInfo)
    {
      if (_persistenceCalculator.WasPersistedRecently(storeInfo.StoreNumber))
      {
        Logger.Debug($"Skipping recently processed Stored; storeNumber={storeInfo.StoreNumber};");        
        return;
      }
      
      await SaveStoreAsync(storeInfo);

      _persistenceCalculator.PreventFuturePersistence(storeInfo.StoreNumber);
    }

    private async Task SaveStoreAsync(StoreInfo storeInfo)
    {
      Logger.Info($"Saving Store; storeNumber={storeInfo.StoreNumber};");

      var shouldUpdateExistingStore = await _dataService.ContainsStoreAsync(storeInfo.StoreNumber);
      if (shouldUpdateExistingStore)
      {
        await UpdateStoreInfoAsync(storeInfo);
      }
      else
      {
        await CreateStoreInfo(storeInfo);       
      }
    }

    private async Task CreateStoreInfo(StoreInfo storeInfo)
    {
      Logger.Debug($"Creating new Store; storeNumber={storeInfo.StoreNumber};");
      await _dataService.CreateNewAsync(storeInfo);
    }

    private async Task UpdateStoreInfoAsync(StoreInfo storeInfo)
    {
      Logger.Debug($"Updating Existing Store; storeNumber={storeInfo.StoreNumber};");
      await _dataService.UpdateAsync(storeInfo);
    }
  }
}