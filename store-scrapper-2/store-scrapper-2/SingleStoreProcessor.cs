using System;
using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;

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
    
    public async Task Process(string fullStoreNumber)
    {
      var correlationId = GenerateUniqueLogId();
      
      LogInfo(correlationId, $"Processing; fullStoreNumber={fullStoreNumber}");
      var storeNumber = StoreInfoRequest.FromFullStoreNumber(fullStoreNumber);

      LogInfo(correlationId, "Downloading Store");     
      var storeInfo = await _downloader.DownloadAsync(storeNumber);

      LogInfo(correlationId, "Saving Response");
      var shouldUpdateExistingStore = await _dataService.ContainsStoreAsync(
        storeNumber.StoreNumber,
        storeNumber.SatelliteNumber
      );

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