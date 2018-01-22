using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using store_scrapper_2.Configuration;

namespace store_scrapper_2.Services
{
  public class AllZipCodesProcessor : IAllZipCodesProcessor
  {
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    
    private readonly IZipCodeDataService _zipCodeDataService;
    private readonly ISingleZipCodeProcessor _singleZipCodeProcessor;
    private readonly IDelaySimulator _delaySimulator;
    private readonly IConfigurationReader _configurationReader;

    public AllZipCodesProcessor(
      IZipCodeDataService zipCodeDataService,
      ISingleZipCodeProcessor singleZipCodeProcessor,
      IDelaySimulator delaySimulator,
      IConfigurationReader configurationReader)
    {
      _zipCodeDataService = zipCodeDataService;
      _singleZipCodeProcessor = singleZipCodeProcessor;
      _delaySimulator = delaySimulator;
      _configurationReader = configurationReader;
    }

    public async Task ProcessAsync()
    {
      var zipCodes = (await _zipCodeDataService.AllAsync())
        .OrderBy(_ => _.UpdateTimeUtc)
        .Select(_ => _.ZipCode)
        .ToArray();

      do
      {
        Logger.Info($"ProcessAsync; count={zipCodes.Length}");

        foreach (var zipCode in zipCodes)
        {
          await _singleZipCodeProcessor.ProcessAsync(zipCode);
          await _delaySimulator.Delay();
        }
      } while (_configurationReader.ReadBool(ConfigurationKeys.ZipCodesRunContinuosly));
    }
  }
}