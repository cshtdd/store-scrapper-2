using System.Linq;
using System.Threading.Tasks;
using store_scrapper_2.Model;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Logging;

namespace store_scrapper_2.Services
{
  public class SingleZipCodeProcessor : ISingleZipCodeProcessor
  {
    private readonly IStoreInfoDownloader _downloader;
    private readonly IStoresPersistor _storesPersistor;
    private readonly IZipCodeDataService _zipCodeDataService;

    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public SingleZipCodeProcessor(
      IStoreInfoDownloader downloader,
      IStoresPersistor storesPersistor,
      IZipCodeDataService zipCodeDataService)
    {
      _downloader = downloader;
      _storesPersistor = storesPersistor;
      _zipCodeDataService = zipCodeDataService;
    }

    public async Task ProcessAsync(ZipCode zipCode)
    {     
      Logger.LogInfo("Processing", nameof(zipCode), zipCode.Zip);

      Logger.LogInfo("Downloading Stores", nameof(zipCode), zipCode.Zip);
      var stores = (await _downloader.DownloadAsync(zipCode)).ToArray();
      Logger.LogDebug("Stores Data Downloaded", "storesCount", stores.Length, nameof(zipCode), zipCode.Zip);

      await _storesPersistor.PersistAsync(stores);
      
      await _zipCodeDataService.UpdateZipCodeAsync(zipCode.Zip);

      Logger.LogInfo("Processing", 
        "Result", true, 
        nameof(zipCode), zipCode.Zip, 
        "Latitude", zipCode.Latitude.ToString("F8"),
        "Longitude", zipCode.Longitude.ToString("F8"));
    }
  }
}