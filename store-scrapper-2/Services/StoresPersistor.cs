using System.Collections.Generic;
using System.Linq;

using store_scrapper_2.DataTransmission;
using store_scrapper_2.Logging;

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

    public void Persist(IEnumerable<StoreInfo> storesEnumerableParam)
    {
      var allStores = (storesEnumerableParam ?? new StoreInfo[]{}).ToArray();      
      var storesToPersist = allStores
        .Where(_ => !_persistenceCalculator.WasPersistedRecently(_.StoreNumber))
        .ToArray();
      var numbersToPersist = storesToPersist.Select(_ => _.StoreNumber).ToArray();
      
      Logger.LogInfo("Saving Stores", "totalCount", allStores.Length, "count", numbersToPersist.Length);
      
      if (numbersToPersist.Length == 0)
      {
        return;
      }

      var numbersToUpdate = (_dataService.ContainsStore(numbersToPersist)).ToArray();
      var numbersToCreate = numbersToPersist.Except(numbersToUpdate).ToArray();

      var storesToUpdate = storesToPersist.Where(_ => numbersToUpdate.Contains(_.StoreNumber)).ToArray();
      var storesToCreate = storesToPersist.Where(_ => numbersToCreate.Contains(_.StoreNumber)).ToArray();

      Logger.LogDebug("Creating Stores", "count", storesToCreate.Length);
      _dataService.CreateNew(storesToCreate);

      Logger.LogDebug("Updating Stores", "count", storesToUpdate.Length);
      _dataService.Update(storesToUpdate);
      
      foreach (var storeNumber in numbersToPersist)
      {
        _persistenceCalculator.PreventFuturePersistence(storeNumber);
      }
    }
  }
}