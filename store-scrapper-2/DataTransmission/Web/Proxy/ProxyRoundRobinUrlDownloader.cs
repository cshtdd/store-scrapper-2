using System.Net;
using store_scrapper_2.Configuration;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyRoundRobinUrlDownloader : IProxyRoundRobinUrlDownloader
  {
    private readonly IProxyRepository _proxyRepository;
    private readonly IProxiedUrlDownloader _proxiedDownloader;
    private readonly IConfigurationReader _configurationReader;

    public ProxyRoundRobinUrlDownloader(
      IProxyRepository proxyRepository,
      IProxiedUrlDownloader proxiedDownloader,
      IConfigurationReader configurationReader)
    {
      _proxyRepository = proxyRepository;
      _proxiedDownloader = proxiedDownloader;
      _configurationReader = configurationReader;
    }

    public string Download(string url)
    {
      var maxAttempts = ReadMaxAttempts();

      for (int i = 0; i < maxAttempts; i++)
      {
        try
        {
          return DownloadInternal(url);
        }
        catch (WebException)
        {
          
        }
      }

      return "";
    }

    private int ReadMaxAttempts() => _configurationReader.ReadInt(ConfigurationKeys.ProxyUrlMaxAttempts, 10);

    private string DownloadInternal(string url)
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
  }
}