using System.Collections.Generic;
using System.Linq;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyReadingStrategyRoundRobin : IProxyReadingStrategy
  {
    private int lastReadIndex;
    
    public void Reset()
    {
      throw new System.NotImplementedException();
    }

    public ProxyInfo Read(IEnumerable<ProxyStatistics> proxies)
    {
      var proxiesArray = proxies.ToArray();
      
      return proxiesArray[lastReadIndex++ % proxiesArray.Length].Proxy;
    }
  }
}