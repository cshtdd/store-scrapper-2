using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2.Services
{
  public class SingleStorePersistor : ISingleStorePersistor
  {
    private readonly IStoreInfoResponseDataService _dataService;
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public SingleStorePersistor(IStoreInfoResponseDataService dataService) => _dataService = dataService;

    public async Task PersistAsync(StoreInfo storeInfo)
    {
      Logger.Info($"Saving Response; storeNumber={storeInfo.StoreNumber};");
      var shouldUpdateExistingStore = await _dataService.ContainsStoreAsync(storeInfo.StoreNumber);

      if (shouldUpdateExistingStore)
      {
        Logger.Info($"Updating Existing Store; storeNumber={storeInfo.StoreNumber};");
        await _dataService.UpdateAsync(storeInfo);
      }
      else
      {
        Logger.Info($"Creating new Store; storeNumber={storeInfo.StoreNumber};");
        await _dataService.CreateNewAsync(storeInfo);
      }
    }
  }
}