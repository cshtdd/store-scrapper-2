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
      var storeNumber = StoreInfoRequest.FromFullStoreNumber(fullStoreNumber);
      await _downloader.DownloadAsync(storeNumber);
    }
  }
}