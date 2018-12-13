using System;
using System.Collections.Generic;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyRepository : IProxyRepository
  {
    private int lastReadIndex = 0;
    private readonly List<ProxyInfo> proxies = new List<ProxyInfo>();
    
    private readonly IProxyListReader _proxyListReader;

    public ProxyRepository(IProxyListReader proxyListReader)
    {
      _proxyListReader = proxyListReader;
    }

    public ProxyInfo Read()
    {
      if (proxies.Count == 0)
      {
        lastReadIndex = 0;
        proxies.AddRange(_proxyListReader.Read()); 
      }

      return proxies[lastReadIndex++ % proxies.Count];
    }

    public void MarkGoodRequest(ProxyInfo proxy)
    {
      throw new InvalidOperationException("No Proxy has been read");
    }

    public void MarkBadRequest(ProxyInfo proxy)
    {
      throw new InvalidOperationException("No Proxy has been read");
    }
  }
}