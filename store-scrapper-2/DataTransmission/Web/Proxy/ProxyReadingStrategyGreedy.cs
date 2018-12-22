using System.Collections.Generic;
using System.Linq;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyReadingStrategyGreedy : IProxyReadingStrategy
  {
    public void Reset(){ }

    public ProxyInfo Read(IEnumerable<ProxyStatistics> proxies) => 
      proxies
        .OrderBy(s => s.SuccessCount)
        .Last()
        .Proxy;
  }
}