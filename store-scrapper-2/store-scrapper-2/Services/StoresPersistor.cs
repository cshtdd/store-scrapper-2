﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2.Services
{
  public class StoresPersistor : IStoresPersistor
  {
    private readonly IStoreInfoResponseDataService _dataService;
    private readonly IStorePersistenceCalculator _persistenceCalculator;
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public StoresPersistor(IStoreInfoResponseDataService dataService,
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

    public async Task PersistAsync(IEnumerable<StoreInfo> storesEnumerableParam)
    {
      var allStores = (storesEnumerableParam ?? new StoreInfo[]{}).ToArray();
      var storesToPersist = allStores
        .Where(_ => !_persistenceCalculator.WasPersistedRecently(_.StoreNumber))
        .ToArray();
      var numbersToPersist = storesToPersist.Select(_ => _.StoreNumber);

      if (!numbersToPersist.Any())
      {
        return;
      }

      var numbersToUpdate = (await _dataService.ContainsStoreAsync(numbersToPersist)).ToArray();
      var numbersToCreate = numbersToPersist.Except(numbersToUpdate).ToArray();

      var storesToUpdate = storesToPersist.Where(_ => numbersToUpdate.Contains(_.StoreNumber)).ToArray();
      var storesToCreate = storesToPersist.Where(_ => numbersToCreate.Contains(_.StoreNumber)).ToArray();

      await _dataService.CreateNewAsync(storesToCreate);
      await _dataService.UpdateAsync(storesToUpdate);
      
      foreach (var storeNumber in numbersToPersist)
      {
        _persistenceCalculator.PreventFuturePersistence(storeNumber);
      }
    }

    private async Task SaveStoreAsync(StoreInfo storeInfo)
    {
      Logger.Info($"Saving Store; storeNumber={storeInfo.StoreNumber};");

      var shouldUpdateExistingStore = (await _dataService.ContainsStoreAsync(new [] {storeInfo.StoreNumber})).Any();
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
      await _dataService.CreateNewAsync(new [] { storeInfo });
    }

    private async Task UpdateStoreInfoAsync(StoreInfo storeInfo)
    {
      Logger.Debug($"Updating Existing Store; storeNumber={storeInfo.StoreNumber};");
      await _dataService.UpdateAsync(new [] { storeInfo });
    }
  }
}