using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2.Services
{
  public class SingleStorePersistor : ISingleStorePersistor
  {
    private readonly IStoreInfoResponseDataService _dataService;
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public SingleStorePersistor(IStoreInfoResponseDataService dataService) => _dataService = dataService;

    public async Task PersistAsync(StoreInfoResponse store)
    {
      Logger.Info($"Saving Response; storeNumber={store.StoreNumber};");
      var shouldUpdateExistingStore = await _dataService.ContainsStoreAsync(store.StoreNumber);

      if (shouldUpdateExistingStore)
      {
        Logger.Info($"Updating Existing Store; storeNumber={store.StoreNumber};");
        await _dataService.UpdateAsync(store);
      }
      else
      {
        Logger.Info($"Creating new Store; storeNumber={store.StoreNumber};");
        await _dataService.CreateNewAsync(store);
      }
    }
  }
}