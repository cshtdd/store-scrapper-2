using System;
using System.IO;
using System.Net;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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

    [Fact]
    public void BubblesUpWebExceptions()
    {
      _webRequestFactory.CreateHttp(Arg.Any<string>(), Arg.Any<ProxyInfo>())
        .Returns(WebRequest.CreateHttp("https://tddapps.com"));
      
      _webRequestExecutor.Run(Arg.Any<WebRequest>())
        .Throws(new WebException("download error"));
      
      ((Action) (() =>
        {
          _downloader.Download("my url", new ProxyInfo("192.168.1.1:8080"));
        }))
        .Should()
        .Throw<WebException>();
    }
    
    [Fact]
    public void BubblesUpIOExceptionsAsWebExceptions()
    {
      _webRequestFactory.CreateHttp(Arg.Any<string>(), Arg.Any<ProxyInfo>())
        .Returns(WebRequest.CreateHttp("https://tddapps.com"));
      
      _webRequestExecutor.Run(Arg.Any<WebRequest>())
        .Throws(new IOException("socket closed"));
      
      ((Action) (() =>
        {
          _downloader.Download("my url", new ProxyInfo("192.168.1.1:8080"));
        }))
        .Should()
        .Throw<WebException>();
    }
    
    [Fact]
    public void BubblesUpOperationCanceledExceptionsAsWebExceptions()
    {
      _webRequestFactory.CreateHttp(Arg.Any<string>(), Arg.Any<ProxyInfo>())
        .Returns(WebRequest.CreateHttp("https://tddapps.com"));
      
      _webRequestExecutor.Run(Arg.Any<WebRequest>())
        .Throws(new OperationCanceledException("the request was canceled"));
      
      ((Action) (() =>
        {
          _downloader.Download("my url", new ProxyInfo("192.168.1.1:8080"));
        }))
        .Should()
        .Throw<WebException>();
    }
  }
}