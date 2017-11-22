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

    public async Task Process(string fullStoreNumber)
    {
      Console.WriteLine($"Processing; fullStoreNumber={fullStoreNumber}");
      var storeNumber = StoreInfoRequest.FromFullStoreNumber(fullStoreNumber);

      Console.WriteLine($"Downloading Store; storeNumber={storeNumber}");     
      var storeInfo = await _downloader.DownloadAsync(storeNumber);

      await SaveStoreInfo(storeNumber, storeInfo);
    }

    private async Task SaveStoreInfo(StoreInfoRequest storeNumber, StoreInfoResponse storeInfo)
    {
      Console.WriteLine($"Saving Response; storeNumber={storeNumber}");
      var shouldUpdateExistingStore = await _dataService.ContainsStoreAsync(
        storeNumber.StoreNumber,
        storeNumber.SatelliteNumber
      );

      if (shouldUpdateExistingStore)
      {
        Console.WriteLine($"Updating Existing Store Info");
        await _dataService.UpdateAsync(storeInfo);
      }
      else
      {
        Console.WriteLine($"Creating new Store Info");
        await _dataService.CreateNewAsync(storeInfo);
      }
    }
  }
}