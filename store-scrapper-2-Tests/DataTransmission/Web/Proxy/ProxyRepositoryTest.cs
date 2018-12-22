using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2;
using store_scrapper_2.Configuration;
using store_scrapper_2.DataTransmission.Web;
using store_scrapper_2.DataTransmission.Web.Proxy;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission.Web.Proxy
{
  public class ProxyRepositoryTest
  {
    private readonly IProxyListReader _proxyListReader = Substitute.For<IProxyListReader>();
    private readonly IConfigurationReader _configurationReader = Substitute.For<IConfigurationReader>();
    private readonly IProxyReadingStrategy _proxyReadingStrategy = Substitute.For<IProxyReadingStrategy>();
    
    private readonly ProxyRepository _repository;
    
    public ProxyRepositoryTest()
    {
      _repository = new ProxyRepository(_proxyListReader, _proxyReadingStrategy, _configurationReader); 
    }
    
    [Fact]
    public void CannotMarkGoodRequestsIfNoProxiesHaveBeenRead()
    {
      ((Action)(() =>
      {
        _repository.CountSuccessRequest("127.0.0.1:80");
      })).Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void CannotMarkGoodRequestsIfTheProxyIsNotFound()
    {
      _proxyListReader.Read().Returns(new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      });

      _repository.Read();
      
      ((Action)(() =>
      {
        _repository.CountSuccessRequest("127.0.0.1:80");
      })).Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void CannotMarkBadRequestsIfNoProxiesHaveBeenRead()
    {
      ((Action)(() =>
      {
        _repository.CountFailedRequest("127.0.0.1:80");
      })).Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void CannotMarkBadRequestsIfTheProxyIsNotFound()
    {
      _proxyListReader.Read().Returns(new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      });

      _repository.Read();
      
      ((Action)(() =>
      {
        _repository.CountFailedRequest("127.0.0.1:80");
      })).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MultipleReadCallsDoNotCauseMultipleListRetrievals()
    {
      _proxyListReader.Read().Returns(new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      });

      for (int i = 0; i < 10; i++)
      {
        _repository.Read();
      }
      
      _proxyListReader.Received(1).Read();
    }

    [Fact]
    public void RetrievingTheProxiesResetsTheProxyReadingStrategy()
    {
      _proxyListReader.Read().Returns(new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      });

      for (int i = 0; i < 10; i++)
      {
        _repository.Read();
      }
      
      _proxyReadingStrategy.Received(1).Reset();
    }

    [Fact]
    public void ReadReturnsTheReadingStrategyResult()
    {
      var seededProxies = new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      };
      _proxyListReader.Read().Returns(seededProxies);

      _proxyReadingStrategy
        .Read(Arg.Is<IEnumerable<ProxyStatistics>>(stats => 
          stats.Select(s => s.Proxy).SequenceEqual(seededProxies)))
        .Returns("10.0.0.1:9000");

      _repository.Read().Should().Be((ProxyInfo) "10.0.0.1:9000");
    }
    
    [Fact]
    public void RemovesTroublesomeProxiesFromTheRotation()
    {
      SeedFailThreshold(2);
      SeedProxyMaxCount(200);
      
      _proxyListReader.Read().Returns(new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      });

      var roundRobin = new ProxyReadingStrategyRoundRobin();
      _proxyReadingStrategy.Read(Arg.Any<IEnumerable<ProxyStatistics>>())
        .Returns(s => roundRobin.Read(s.Arg<IEnumerable<ProxyStatistics>>()));

      _repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");
      
      _repository.CountFailedRequest("192.168.1.2:8080");
      
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");

      _repository.CountFailedRequest("192.168.1.2:8080");

      _repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");
    }

    [Fact]
    public void RemoveProxiesThatHaveBeenUsedManyTimes()
    {
      SeedFailThreshold(200);
      SeedProxyMaxCount(2);
      
      _proxyListReader.Read().Returns(new ProxyInfo[]
      {
        "192.168.1.1:8080",
        "192.168.1.2:8080",
        "192.168.1.3:8080"
      });
      
      var roundRobin = new ProxyReadingStrategyRoundRobin();
      _proxyReadingStrategy.Read(Arg.Any<IEnumerable<ProxyStatistics>>())
        .Returns(s => roundRobin.Read(s.Arg<IEnumerable<ProxyStatistics>>()));
      
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");
      
      _repository.CountSuccessRequest("192.168.1.2:8080");
      
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");

      _repository.CountSuccessRequest("192.168.1.2:8080");

      _repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");
    }

    [Fact]
    public void RetrievesTheProxyListOnceAllProxiesAreRemoved()
    {
      SeedFailThreshold(1);
      SeedProxyMaxCount(1);

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
      _proxyListReader.Read().Returns(proxyList1, proxyList2);
      
      var roundRobin = new ProxyReadingStrategyRoundRobin();
      _proxyReadingStrategy.Read(Arg.Any<IEnumerable<ProxyStatistics>>())
        .Returns(s => roundRobin.Read(s.Arg<IEnumerable<ProxyStatistics>>()));
     
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.1:8080");
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.2:8080");
      _repository.Read().Should().Be((ProxyInfo)"192.168.1.3:8080");

      _repository.CountSuccessRequest("192.168.1.1:8080");
      _repository.CountSuccessRequest("192.168.1.2:8080");
      _repository.CountSuccessRequest("192.168.1.3:8080");

      _repository.Read();
      
      _proxyReadingStrategy.Received(2).Reset();
      _proxyListReader.Received(2).Read();
    }

    private void SeedFailThreshold(uint value) => _configurationReader
      .ReadUInt(ConfigurationKeys.ProxyFailThreshold, 5)
      .Returns(value);

    private void SeedProxyMaxCount(uint value) => _configurationReader
      .ReadUInt(ConfigurationKeys.ProxyMaxCount, 100)
      .Returns(value);
  }
}