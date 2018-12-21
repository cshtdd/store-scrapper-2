using FluentAssertions;
using store_scrapper_2.DataTransmission.Web;
using store_scrapper_2.DataTransmission.Web.Proxy;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission.Web.Proxy
{
  public class ProxyReadingStrategyRoundRobinTest
  {
    private readonly ProxyReadingStrategyRoundRobin strategy = new ProxyReadingStrategyRoundRobin();

    [Fact]
    public void CirclesTheProxies()
    {
      var proxies = new[]
      {
        new ProxyStatistics("192.168.1.1:8080", 100, 2), 
        new ProxyStatistics("192.168.1.2:8080", 100, 2), 
        new ProxyStatistics("192.168.1.3:8080", 100, 2), 
      };

      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.1:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.2:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.3:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.1:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.2:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.3:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.1:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.2:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.3:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.1:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.2:8080");
      strategy.Read(proxies).Should().Be((ProxyInfo) "192.168.1.3:8080");
    }
  }
}