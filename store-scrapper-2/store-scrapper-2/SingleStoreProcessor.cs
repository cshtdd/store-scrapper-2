using System;
using System.Threading.Tasks;
using store;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2
{
  public class SingleStoreProcessor
  {
    private readonly IStoreInfoDownloader _downloader;
    private readonly IStoreInfoResponseDataService _dataService; 

    public SingleStoreProcessor(IStoreInfoDownloader downloader, IStoreInfoResponseDataService dataService)
    {
      _downloader = downloader;
      _dataService = dataService;
    }

    private string GenerateUniqueLogId() => Guid.NewGuid().ToString("N");
    private void Log(string id, string logString) => Console.WriteLine($"{GetType().Name}; {logString}; correlationId={id};");
    
    public async Task Process(string fullStoreNumber)
    {
      var correlationId = GenerateUniqueLogId();
      
      Log(correlationId, $"Processing; fullStoreNumber={fullStoreNumber}");
      var storeNumber = StoreInfoRequest.FromFullStoreNumber(fullStoreNumber);

      Log(correlationId, $"Downloading Store");     
      var storeInfo = await _downloader.DownloadAsync(storeNumber);

      Log(correlationId, $"Saving Response");
      var shouldUpdateExistingStore = await _dataService.ContainsStoreAsync(
        storeNumber.StoreNumber,
        storeNumber.SatelliteNumber
      );

      if (shouldUpdateExistingStore)
      {
        Log(correlationId, $"Updating Existing Store");
        await _dataService.UpdateAsync(storeInfo);
      }
      else
      {
        Log(correlationId, $"Creating new Store");
        await _dataService.CreateNewAsync(storeInfo);
      }
    }
  }
}