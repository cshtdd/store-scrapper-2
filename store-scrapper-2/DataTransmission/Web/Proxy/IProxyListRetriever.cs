using System.Collections.Generic;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public interface IProxyListRetriever
  {
    IEnumerable<ProxyInfo> Read();
  }
}