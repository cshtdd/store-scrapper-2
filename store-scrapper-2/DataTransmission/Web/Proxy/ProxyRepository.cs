using System;
using System.Collections.Generic;
using store_scrapper_2.Configuration;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyRepository : IProxyRepository
  {
    private int lastReadIndex = 0;
    private readonly List<ProxyInfo> proxies = new List<ProxyInfo>();
    
    private readonly IProxyListReader _proxyListReader;
    private readonly IConfigurationReader _configurationReader;

    public ProxyRepository(IProxyListReader proxyListReader, IConfigurationReader configurationReader)
    {
      _proxyListReader = proxyListReader;
      _configurationReader = configurationReader;
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