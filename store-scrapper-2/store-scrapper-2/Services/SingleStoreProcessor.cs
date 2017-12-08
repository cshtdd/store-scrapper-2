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

    public async Task Process(StoreNumber storeNumber)
    {     
      Logger.Info($"Processing; fullStoreNumber={storeNumber};");
      var storeInfoRequest = new StoreInfoRequest(storeNumber);

      Logger.Info("Downloading Store;");     
      var storeInfo = await _downloader.DownloadAsync(storeInfoRequest);

      Logger.Info("Saving Response;");
      var shouldUpdateExistingStore = await _dataService.ContainsStoreAsync(storeNumber);

      if (shouldUpdateExistingStore)
      {
        Logger.Info("Updating Existing Store;");
        await _dataService.UpdateAsync(storeInfo);
      }
      else
      {
        Logger.Info("Creating new Store;");
        await _dataService.CreateNewAsync(storeInfo);
      }
    }
  }
}