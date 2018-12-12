using System.Net;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2.DataTransmission.Web;
using store_scrapper_2.DataTransmission.Web.Support;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission.Web
{
  public class UrlDownloaderTest
  {
    [Fact]
    public void DownloadsAUrl()
    {
      var targetRequest = WebRequest.CreateHttp("https://tddapps.com");
      
      var factory = Substitute.For<IWebRequestFactory>();
      factory.CreateHttp("my url")
        .Returns(targetRequest);

      var requestExecutor = Substitute.For<IWebRequestExecutor>();
      requestExecutor.Run(targetRequest)
        .Returns("tdd rocks");

      
      new UrlDownloader(requestExecutor, factory)
        .Download("my url")
        .Should()
        .Be("tdd rocks");
    }
  }
}