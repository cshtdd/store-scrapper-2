using System;
using System.Net;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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
    private readonly IUrlDownloader urlDownloader = Substitute.For<IUrlDownloader>();

    private readonly ProxyRoundRobinUrlDownloader downloader;
    
    public ProxyRoundRobinUrlDownloaderTest()
    {
      configurationReader.ReadInt(ConfigurationKeys.ProxyUrlMaxAttempts, 10).Returns(2);

      downloader = new ProxyRoundRobinUrlDownloader(
        proxyRepository, 
        proxiedDownloader,
        configurationReader,
        urlDownloader
      );
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
      proxyRepository.Read().Returns("192.168.1.1:8080");

      downloader.Download("https://tddapps.com");

      proxiedDownloader.Received().Download("https://tddapps.com", "192.168.1.1:8080");
    }

    [Fact]
    public void ReturnsTheDownloadedResult()
    {
      proxyRepository.Read().Returns("192.168.1.1:8080");
      proxiedDownloader.Download("https://tddapps.com", "192.168.1.1:8080")
        .Returns("tdd rocks");
      
      downloader.Download("https://tddapps.com")
        .Should()
        .Be("tdd rocks");
    }

    [Fact]
    public void CountsSuccessfulDownloads()
    {
      proxyRepository.Read().Returns("192.168.1.1:8080");
      proxiedDownloader.Download("https://tddapps.com", "192.168.1.1:8080")
        .Returns("tdd rocks");

      downloader.Download("https://tddapps.com");
      
      proxyRepository.Received().CountSuccessRequest("192.168.1.1:8080");
      proxyRepository.DidNotReceive().CountFailedRequest(Arg.Any<ProxyInfo>());
    }
    
    [Fact]
    public void CountsFailedDownloads()
    {
      proxyRepository.Read().Returns("192.168.1.1:8080");
      proxiedDownloader.Download("https://tddapps.com", "192.168.1.1:8080").Throws<WebException>();

      ((Action)(() =>
      {
        downloader.Download("https://tddapps.com");
      })).Should()
      .Throw<WebException>();
      
      proxyRepository.DidNotReceive().CountSuccessRequest(Arg.Any<ProxyInfo>());
      proxyRepository.Received().CountFailedRequest("192.168.1.1:8080");
    }
    
    [Fact]
    public void AttemptsMultipleDownloadOnFailures()
    {
      proxyRepository.Read().Returns(
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080",
        "192.168.1.4:8080"
      );
      
      proxiedDownloader.Download("https://tddapps.com", "192.168.1.1:8080").Throws<WebException>();
      proxiedDownloader.Download("https://tddapps.com", "192.168.1.2:8080").Returns("uff, barely");

      downloader.Download("https://tddapps.com")
        .Should()
        .Be("uff, barely");
      
      proxyRepository.Received().CountFailedRequest("192.168.1.1:8080");
      proxyRepository.Received().CountSuccessRequest("192.168.1.2:8080");
    }

    [Fact]
    public void BubblesUpExceptionWhenMaxNumberOfDownloadAttemptsExceeded()
    {
      proxyRepository.Read().Returns(
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080",
        "192.168.1.4:8080"
      );
      
      proxiedDownloader.Download("https://tddapps.com", "192.168.1.1:8080").Throws<WebException>();
      proxiedDownloader.Download("https://tddapps.com", "192.168.1.2:8080").Throws<WebException>();
      proxiedDownloader.Download("https://tddapps.com", "192.168.1.3:8080").Returns("will not run");

      ((Action)(() =>
      {
        downloader.Download("https://tddapps.com");
      })).Should()
      .Throw<WebException>();
      
      proxyRepository.Received().CountFailedRequest("192.168.1.1:8080");
      proxyRepository.Received().CountFailedRequest("192.168.1.2:8080");
    }
  }
}