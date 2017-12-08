using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public class SingleStoreProcessor
  {
    private readonly IStoreInfoDownloader _downloader;
    private readonly ISingleStorePersistor _singleStorePersistor;

    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public SingleStoreProcessor(IStoreInfoDownloader downloader, ISingleStorePersistor singleStorePersistor)
    {
      _downloader = downloader;
      _singleStorePersistor = singleStorePersistor;
    }

    public async Task ProcessAsync(StoreNumber storeNumber)
    {     
      Logger.Info($"Processing; fullStoreNumber={storeNumber};");
      var storeInfoRequest = new StoreInfoRequest(storeNumber);

      Logger.Info("Downloading Stores;");     
      var stores = await _downloader.DownloadAsync(storeInfoRequest);

      foreach (var store in stores)
      {
        await _singleStorePersistor.PersistAsync(store);
      }
    }
  }
}