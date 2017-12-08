using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public class SingleStoreProcessor
  {
    private readonly IStoreInfoDownloader _downloader;
    private readonly IStoreInfoResponseDataService _dataService;
    
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public SingleStoreProcessor(IStoreInfoDownloader downloader, IStoreInfoResponseDataService dataService)
    {
      _downloader = downloader;
      _dataService = dataService;
    }

    public async Task ProcessAsync(StoreNumber storeNumber)
    {     
      Logger.Info($"Processing; fullStoreNumber={storeNumber};");
      var storeInfoRequest = new StoreInfoRequest(storeNumber);

      Logger.Info("Downloading Stores;");     
      var stores = await _downloader.DownloadAsync(storeInfoRequest);

      foreach (var store in stores)
      {
        await PersistSingleStoreAsync(store);
      }
    }

    private async Task PersistSingleStoreAsync(StoreInfoResponse store)
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