using NSubstitute;
using store_scrapper_2.Configuration;
using store_scrapper_2.DataTransmission.Web.Proxy;

namespace store_scrapper_2_Tests.DataTransmission.Web.Proxy
{
  public class ProxyRoundRobinUrlDownloaderTest
  {
    private readonly IProxyRepository proxyRepository = Substitute.For<IProxyRepository>();
    private readonly IProxiedUrlDownloader proxiedDownloader = Substitute.For<IProxiedUrlDownloader>();
    private readonly IConfigurationReader configurationReader = Substitute.For<IConfigurationReader>();

    private readonly ProxyRoundRobinUrlDownloader downloader;
    
    public ProxyRoundRobinUrlDownloaderTest()
    {
      downloader = new ProxyRoundRobinUrlDownloader(proxyRepository, proxiedDownloader, configurationReader);
    }
    
    
  }
}