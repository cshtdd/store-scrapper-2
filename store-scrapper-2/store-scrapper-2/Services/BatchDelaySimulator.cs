using System.Reflection;
using System.Threading.Tasks;
using log4net;
using store_scrapper_2.Configuration;

namespace store_scrapper_2.Services
{
  public class BatchDelaySimulator : IBatchDelaySimulator
  {
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    
    private readonly IConfigurationReader _configurationReader;

    public BatchDelaySimulator(IConfigurationReader configurationReader) => _configurationReader = configurationReader;

    public async Task Delay()
    {
      var delayMs = _configurationReader.ReadUInt(ConfigurationKeys.ZipCodesDelayMs);
      Logger.Debug($"Sleeping...; {nameof(delayMs)}={delayMs};");
      await Task.Delay((int) delayMs);
    }
  }
}