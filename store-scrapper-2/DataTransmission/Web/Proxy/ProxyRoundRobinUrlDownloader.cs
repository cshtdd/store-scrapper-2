using System.Net;
using store_scrapper_2.Configuration;
using store_scrapper_2.Logging;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyRoundRobinUrlDownloader : IProxyRoundRobinUrlDownloader
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly IProxyRepository _proxyRepository;
    private readonly IProxiedUrlDownloader _proxiedDownloader;
    private readonly IConfigurationReader _configurationReader;
    private readonly IUrlDownloader _urlDownloader;

    public ProxyRoundRobinUrlDownloader(IProxyRepository proxyRepository,
      IProxiedUrlDownloader proxiedDownloader,
      IConfigurationReader configurationReader,
      IUrlDownloader urlDownloader)
    {
      _proxyRepository = proxyRepository;
      _proxiedDownloader = proxiedDownloader;
      _configurationReader = configurationReader;
      _urlDownloader = urlDownloader;
    }

    public string Download(string url)
    {
      var maxAttempts = ReadMaxAttempts();

      for (var i = 0; i < maxAttempts; i++)
      {
        try
        {
          return ProxiedDownload(url, i);
        }
        catch (WebException){ }
      }
      
      return RegularDownload(url);
    }

    private string ProxiedDownload(string url, int i)
    {
      var result = ProxiedDownload(url);
      Logger.LogInfo("ProxiedDownload", "Success", true, "Attempts", i + 1, "Url", url);
      return result;
    }

    private uint ReadMaxAttempts() => _configurationReader.ReadUInt(ConfigurationKeys.ProxyUrlMaxAttempts, 10);

    private string ProxiedDownload(string url)
    {
      var proxy = _proxyRepository.Read();
      try
      {
        var result = _proxiedDownloader.Download(url, proxy);
        _proxyRepository.CountSuccessRequest(proxy);
        return result;
      }
      catch
      {
        _proxyRepository.CountFailedRequest(proxy);
        throw;
      }
    }
    
    private string RegularDownload(string url)
    {
      var maxAttempts = ReadMaxAttempts();
      Logger.LogInfo("UrlMaxFailedAttemptsReached", nameof(url), url, nameof(maxAttempts), maxAttempts);
      return _urlDownloader.Download(url);
    }
  }
}