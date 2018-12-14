using NSubstitute;
using store_scrapper_2.Configuration;
using store_scrapper_2.DataTransmission.Web;
using store_scrapper_2.DataTransmission.Web.Proxy;
using Xunit;

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

    [Fact]
    public void ReadsAProxyToDownloadAUrl()
    {
      downloader.Download("https://tddapps.com");

      proxyRepository.Received().Read();
    }

    [Fact]
    public void DownloadsTheUrlWithTheReadProxy()
    {
      proxyRepository.Read()
        .Returns((ProxyInfo)"192.168.1.1:8080");

      downloader.Download("https://tddapps.com");

      proxiedDownloader.Received().Download("https://tddapps.com", "192.168.1.1:8080");
    }
  }
}