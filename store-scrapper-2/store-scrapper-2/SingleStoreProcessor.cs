using System;
using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;

namespace store_scrapper_2
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

    private static string GenerateUniqueLogId() => Guid.NewGuid().ToString("N");
    private static void LogInfo(string id, string logString) => Logger.Info($"{logString}; correlationId={id};");
    
    public async Task Process(StoreNumber storeNumber)
    {
      var correlationId = GenerateUniqueLogId();
      
      LogInfo(correlationId, $"Processing; fullStoreNumber={storeNumber}");
      var storeInfoRequest = new StoreInfoRequest(storeNumber);

      LogInfo(correlationId, "Downloading Store");     
      var storeInfo = await _downloader.DownloadAsync(storeInfoRequest);

      LogInfo(correlationId, "Saving Response");
      var shouldUpdateExistingStore = await _dataService.ContainsStoreAsync(storeNumber);

      if (shouldUpdateExistingStore)
      {
        LogInfo(correlationId, "Updating Existing Store");
        await _dataService.UpdateAsync(storeInfo);
      }
      else
      {
        LogInfo(correlationId, "Creating new Store");
        await _dataService.CreateNewAsync(storeInfo);
      }
    }
  }
}