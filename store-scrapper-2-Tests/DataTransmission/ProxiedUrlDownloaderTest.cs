using System.Net;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2.DataTransmission;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission
{
  public class ProxiedUrlDownloaderTest
  {
    [Fact]
    public void DownloadsAUrl()
    {
      var proxy = new ProxyInfo("192.168.1.1:8080");
      var targetRequest = WebRequest.CreateHttp("https://tddapps.com");
      
      var factory = Substitute.For<IWebRequestFactory>();
      factory.CreateHttp("my url", proxy)
        .Returns(targetRequest);

      var requestExecutor = Substitute.For<IWebRequestExecutor>();
      requestExecutor.Run(targetRequest)
        .Returns("tdd rocks");

      
      new ProxiedUrlDownloader(requestExecutor, factory)
        .Download("my url", proxy)
        .Should()
        .Be("tdd rocks");
    }
  }
}