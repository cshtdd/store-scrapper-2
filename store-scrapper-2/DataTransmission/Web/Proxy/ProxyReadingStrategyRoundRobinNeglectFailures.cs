using System.Collections.Generic;
using System.Linq;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyReadingStrategyRoundRobinNeglectFailures : IProxyReadingStrategy
  {
    private readonly ProxyReadingStrategyRoundRobin _roundRobin = new ProxyReadingStrategyRoundRobin();
    
    public void Reset()
    {
      _roundRobin.Reset();
    }

    public ProxyInfo Read(IEnumerable<ProxyStatistics> proxies)
    {
      var filteredList = proxies.Where(s => s.FailedCount == 0);
      return _roundRobin.Read(filteredList);
    }
  }
}