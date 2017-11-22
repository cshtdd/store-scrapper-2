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

    public async Task Process(string storeNumber, string satelliteNumber)
    {
      await _downloader.DownloadAsync(new StoreInfoRequest(storeNumber, satelliteNumber));
    }
  }
}