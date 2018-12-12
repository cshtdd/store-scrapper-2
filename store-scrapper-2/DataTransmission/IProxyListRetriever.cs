using System.Collections.Generic;

namespace store_scrapper_2.DataTransmission
{
  public interface IProxyListRetriever
  {
    IEnumerable<ProxyInfo> Read();
  }
}