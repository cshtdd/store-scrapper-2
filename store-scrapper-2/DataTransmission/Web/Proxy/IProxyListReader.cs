using System.Collections.Generic;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public interface IProxyListReader
  {
    IEnumerable<ProxyInfo> Read();
  }
}