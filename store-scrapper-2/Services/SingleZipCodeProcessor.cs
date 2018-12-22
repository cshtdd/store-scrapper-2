using System.Linq;
using System.Net;

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
    private readonly IWebExceptionHandler _webExceptionHandler;
    private readonly IDeadlockDetector _deadlockDetector;

    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public SingleZipCodeProcessor(IStoreInfoDownloader downloader,
      IStoresPersistor storesPersistor,
      IZipCodeDataService zipCodeDataService,
      IWebExceptionHandler webExceptionHandler,
      IDeadlockDetector deadlockDetector)
    {
      _downloader = downloader;
      _storesPersistor = storesPersistor;
      _zipCodeDataService = zipCodeDataService;
      _webExceptionHandler = webExceptionHandler;
      _deadlockDetector = deadlockDetector;
    }

    public void Process(ZipCode zipCode)
    {     
      Logger.LogInfo("Processing", nameof(zipCode), zipCode.Zip);

      Logger.LogInfo("Downloading Stores", nameof(zipCode), zipCode.Zip);

      StoreInfo[] stores;
      try
      {
        stores = _downloader.Download(zipCode).ToArray();
        Logger.LogDebug("Stores Data Downloaded", "storesCount", stores.Length, nameof(zipCode), zipCode.Zip);
      }
      catch(WebException ex)
      {
        if (_webExceptionHandler.ShouldBubbleUpException(ex))
        {
          throw;
        }
        
        LogFailure(zipCode);
        return;
      }

      _storesPersistor.Persist(stores);
      
      _zipCodeDataService.UpdateZipCode(zipCode.Zip);

      LogSuccess(zipCode);
      
      _deadlockDetector.UpdateStatus();
    }

    private static void LogFailure(ZipCode zipCode) => LogResult(zipCode, false);
    private static void LogSuccess(ZipCode zipCode) => LogResult(zipCode, true);
    
    private static void LogResult(ZipCode zipCode, bool result) => Logger.LogInfo("Processing",
      "Result", result,
      nameof(zipCode), zipCode.Zip,
      "Latitude", zipCode.Latitude.ToString("F8"),
      "Longitude", zipCode.Longitude.ToString("F8"));
  }
}