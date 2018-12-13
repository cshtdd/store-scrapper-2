using System;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2.Configuration;
using store_scrapper_2.DataTransmission.Web;
using store_scrapper_2.DataTransmission.Web.Proxy;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission.Web.Proxy
{
  public class ProxyRepositoryTest
  {
    private readonly IProxyListReader proxyListReader = Substitute.For<IProxyListReader>();
    private readonly IConfigurationReader configurationReader = Substitute.For<IConfigurationReader>();
    
    private readonly ProxyRepository repository;

    public ProxyRepositoryTest()
    {
      repository = new ProxyRepository(proxyListReader, configurationReader); 
    }

    [Fact]
    public void CannotMarkGoodRequestsIfNoProxiesHaveBeenRead()
    {
      ((Action)(() =>
      {
          repository.MarkGoodRequest("127.0.0.1:80");
      })).Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void CannotMarkBadRequestsIfNoProxiesHaveBeenRead()
    {
      ((Action)(() =>
      {
          repository.MarkBadRequest("127.0.0.1:80");
      })).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MultipleReadCallsDoNotCauseMultipleListRetrievals()
    {
      proxyListReader.Read().Returns(new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      });

      for (int i = 0; i < 10; i++)
      {
        repository.Read();
      }
      
      proxyListReader.Received(1).Read();
    }
    
    [Fact]
    public void ReadRotatesTheProxies()
    {
      proxyListReader.Read().Returns(new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      });

      repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");
    }

    [Fact]
    public void RemovesTroublesomeProxiesFromTheRotation()
    {
      configurationReader.ReadInt(ConfigurationKeys.ProxyFailThreshold, 5)
        .Returns(2);
      
      proxyListReader.Read().Returns(new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      });

      repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");
      
      repository.MarkBadRequest("192.168.1.2:8080");
      
      repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");

      repository.MarkBadRequest("192.168.1.2:8080");

      repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");
    }
  }
}