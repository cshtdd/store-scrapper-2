using System;
using System.Collections.Generic;
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
          repository.CountSuccessRequest("127.0.0.1:80");
      })).Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void CannotMarkGoodRequestsIfTheProxyIsNotFound()
    {
      proxyListReader.Read().Returns(new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      });

      repository.Read();
      
      ((Action)(() =>
      {
        repository.CountSuccessRequest("127.0.0.1:80");
      })).Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void CannotMarkBadRequestsIfNoProxiesHaveBeenRead()
    {
      ((Action)(() =>
      {
          repository.CountFailedRequest("127.0.0.1:80");
      })).Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void CannotMarkBadRequestsIfTheProxyIsNotFound()
    {
      proxyListReader.Read().Returns(new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      });

      repository.Read();
      
      ((Action)(() =>
      {
        repository.CountFailedRequest("127.0.0.1:80");
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
      configurationReader.ReadInt(ConfigurationKeys.ProxyMaxCount, 100)
        .Returns(200);
      
      proxyListReader.Read().Returns(new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      });

      repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");
      
      repository.CountFailedRequest("192.168.1.2:8080");
      
      repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");

      repository.CountFailedRequest("192.168.1.2:8080");

      repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");
    }

    [Fact]
    public void RemoveProxiesThatHaveBeenUsedManyTimes()
    {
      configurationReader.ReadInt(ConfigurationKeys.ProxyFailThreshold, 5)
        .Returns(200);
      configurationReader.ReadInt(ConfigurationKeys.ProxyMaxCount, 100)
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
      
      repository.CountSuccessRequest("192.168.1.2:8080");
      
      repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");

      repository.CountSuccessRequest("192.168.1.2:8080");

      repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");
    }

    [Fact]
    public void RetrievesTheProxyListOnceAllProxiesAreRemoved()
    {
      configurationReader.ReadInt(ConfigurationKeys.ProxyFailThreshold, 5)
        .Returns(1);
      configurationReader.ReadInt(ConfigurationKeys.ProxyMaxCount, 100)
        .Returns(1);

      IEnumerable<ProxyInfo> proxyList1 = new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      };
      IEnumerable<ProxyInfo> proxyList2 = new ProxyInfo[]
      {
        "10.0.0.1:9000",
        "10.0.0.2:9000",
        "10.0.0.3:9000",
        "10.0.0.4:9000"
      };
      proxyListReader.Read().Returns(proxyList1, proxyList2);
     
      repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");

      repository.CountSuccessRequest((ProxyInfo)"192.168.1.1:8080");
      repository.CountSuccessRequest((ProxyInfo)"192.168.1.2:8080");
      repository.CountSuccessRequest((ProxyInfo)"192.168.1.3:8080");
      
      repository.Read().Should().Be((ProxyInfo)"10.0.0.1:9000");
      repository.Read().Should().Be((ProxyInfo)"10.0.0.2:9000");
      repository.Read().Should().Be((ProxyInfo)"10.0.0.3:9000");
      repository.Read().Should().Be((ProxyInfo)"10.0.0.4:9000");
    }
  }
}