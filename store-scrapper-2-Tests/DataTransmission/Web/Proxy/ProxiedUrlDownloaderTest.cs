using System.Net;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2.DataTransmission.Web;
using store_scrapper_2.DataTransmission.Web.Proxy;
using store_scrapper_2.DataTransmission.Web.Support;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission.Web.Proxy
{
  public class ProxiedUrlDownloaderTest
  {
    private readonly IWebRequestFactory _webRequestFactory = Substitute.For<IWebRequestFactory>();
    private readonly IWebRequestExecutor _webRequestExecutor = Substitute.For<IWebRequestExecutor>();

    private readonly ProxiedUrlDownloader _downloader;

    public ProxiedUrlDownloaderTest()
    {
      _downloader = new ProxiedUrlDownloader(_webRequestExecutor, _webRequestFactory);
    }
    
    [Fact]
    public void DownloadsAUrl()
    {
      var proxy = new ProxyInfo("192.168.1.1:8080");
      var targetRequest = WebRequest.CreateHttp("https://tddapps.com");
      _webRequestFactory.CreateHttp("my url", proxy)
        .Returns(targetRequest);

      _webRequestExecutor.Run(targetRequest)
        .Returns("tdd rocks");

      
      _downloader
        .Download("my url", proxy)
        .Should()
        .Be("tdd rocks");
    }
  }
}