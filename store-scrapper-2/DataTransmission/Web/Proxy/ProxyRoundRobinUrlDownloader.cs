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
      throw new System.NotImplementedException();
    }
  }
}