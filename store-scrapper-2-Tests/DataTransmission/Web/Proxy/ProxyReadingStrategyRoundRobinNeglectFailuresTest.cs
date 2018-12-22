using FluentAssertions;
using store_scrapper_2.DataTransmission.Web;
using store_scrapper_2.DataTransmission.Web.Proxy;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission.Web.Proxy
{
  public class ProxyReadingStrategyRoundRobinNeglectFailuresTest
  {
    private readonly ProxyReadingStrategyRoundRobinNeglectFailures strategy =
      new ProxyReadingStrategyRoundRobinNeglectFailures();
    
    [Fact]
    public void RoundRobinsTheHostsWithoutFailures()
    {
      var proxies = new []{
        new ProxyStatistics("192.168.1.1:8080", 100, 2, 50, 0), 
        new ProxyStatistics("192.168.1.2:8080", 100, 2, 60, 15), 
        new ProxyStatistics("192.168.1.3:8080", 100, 2, 3, 0), 
        new ProxyStatistics("192.168.1.4:8080", 100, 2, 3, 0),
        new ProxyStatistics("192.168.1.5:8080", 100, 2, 3, 10) 
      };

      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.1:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.3:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.4:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.1:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.3:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.4:8080");
    }
  }
}