using System;
using System.Net;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using store_scrapper_2.DataTransmission.Web;
using store_scrapper_2.DataTransmission.Web.Support;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission.Web
{
  public class UrlDownloaderTest
  {
    private readonly IWebRequestFactory _webRequestFactory = Substitute.For<IWebRequestFactory>();
    private readonly IWebRequestExecutor _webRequestExecutor = Substitute.For<IWebRequestExecutor>();
    
    private readonly UrlDownloader _downloader;

    public UrlDownloaderTest()
    {
      _downloader = new UrlDownloader(_webRequestExecutor, _webRequestFactory);
    }
    
    [Fact]
    public void DownloadsAUrl()
    {
      var targetRequest = WebRequest.CreateHttp("https://tddapps.com");
      _webRequestFactory.CreateHttp("my url")
        .Returns(targetRequest);

      _webRequestExecutor.Run(targetRequest)
        .Returns("tdd rocks");
      
      _downloader
        .Download("my url")
        .Should()
        .Be("tdd rocks");
    }

    [Fact]
    public void BubblesUpWebExceptions()
    {
      _webRequestFactory.CreateHttp("my url")
        .Returns(WebRequest.CreateHttp("https://tddapps.com"));

      _webRequestExecutor.Run(Arg.Any<HttpWebRequest>())
        .Throws(new WebException("download error"));
      
      ((Action) (() => { _downloader.Download("my url"); }))
        .Should()
        .Throw<WebException>();
    }
  }
}