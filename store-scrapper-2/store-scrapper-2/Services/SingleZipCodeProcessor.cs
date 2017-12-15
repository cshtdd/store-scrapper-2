using System.Linq;
using System.Threading.Tasks;
using store_scrapper_2.Model;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2.Services
{
  public class SingleZipCodeProcessor : ISingleZipCodeProcessor
  {
    private readonly IStoreInfoDownloader _downloader;
    private readonly ISingleStorePersistor _singleStorePersistor;

    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public SingleZipCodeProcessor(IStoreInfoDownloader downloader, ISingleStorePersistor singleStorePersistor)
    {
      _downloader = downloader;
      _singleStorePersistor = singleStorePersistor;
    }

    public async Task ProcessAsync(ZipCode zipCode)
    {     
      Logger.Info($"Processing; zipCode={zipCode.Zip};");

      Logger.Info($"Downloading Stores; zipCode={zipCode.Zip};");
      var stores = (await _downloader.DownloadAsync(zipCode)).ToArray();
      Logger.Info($"Stores Data Downloaded; storesCount={stores.Length}; zipCode={zipCode.Zip};");

      var persistStoresTasks = stores.Select(_singleStorePersistor.PersistAsync);
      await Task.WhenAll(persistStoresTasks);

      Logger.Info($"Processing; {zipCode} Result=true; zipCode={zipCode.Zip};");
    }
  }
}