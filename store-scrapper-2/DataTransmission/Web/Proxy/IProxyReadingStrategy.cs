using System.Collections.Generic;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public interface IProxyReadingStrategy
  {
    void Reset();
    ProxyInfo Read(IEnumerable<ProxyStatistics> proxies);
  }
}