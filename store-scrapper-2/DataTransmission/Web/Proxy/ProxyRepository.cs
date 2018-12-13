using System;
using System.Collections.Generic;
using System.Linq;
using store_scrapper_2.Configuration;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyRepository : IProxyRepository
  {
    private int lastReadIndex;
    private readonly List<ProxyStatistics> proxies = new List<ProxyStatistics>();
    
    private readonly IProxyListReader _proxyListReader;
    private readonly IConfigurationReader _configurationReader;

    public ProxyRepository(IProxyListReader proxyListReader, IConfigurationReader configurationReader)
    {
      _proxyListReader = proxyListReader;
      _configurationReader = configurationReader;
    }

    public ProxyInfo Read()
    {
      if (HasNoProxies())
      {
        lastReadIndex = 0;
        var newProxies = _proxyListReader.Read().Select(ToProxyStatistics);
        proxies.AddRange(newProxies); 
      }

      return proxies[lastReadIndex++ % proxies.Count].Proxy;
    }

    public void MarkGoodRequest(ProxyInfo proxy)
    {
      EnsureProxiesHaveBeenRead();

      var statistics = FindStatistics(proxy);
      statistics.GoodRequestCount++;

      if (statistics.GoodRequestCount >= ReadSuccessThreshold())
      {
        proxies.Remove(statistics);
      }
    }

    public void MarkBadRequest(ProxyInfo proxy)
    {
      EnsureProxiesHaveBeenRead();

      var statistics = FindStatistics(proxy);
      statistics.BadRequestCount++;

      if (statistics.BadRequestCount >= ReadFailThreshold())
      {
        proxies.Remove(statistics);
      }
    }

    private int ReadFailThreshold() => _configurationReader.ReadInt(ConfigurationKeys.ProxyFailThreshold, 5);
    private int ReadSuccessThreshold() => _configurationReader.ReadInt(ConfigurationKeys.ProxyMaxCount, 100);

    private bool HasNoProxies() => proxies.Count == 0;
    private void EnsureProxiesHaveBeenRead()
    {
      if (HasNoProxies())
      {
        throw new InvalidOperationException("No Proxy has been read");       
      }
    }

    private static ProxyStatistics ToProxyStatistics(ProxyInfo proxyInfo) => new ProxyStatistics(proxyInfo);

    private ProxyStatistics FindStatistics(ProxyInfo proxy)
    {
      var result = proxies.Find(s => s.Proxy.Equals(proxy));

      if (result == null)
      {
        throw new InvalidOperationException($"Cannot find Proxy:{proxy}");
      }
      
      return result;
    }

    private class ProxyStatistics
    {
      public ProxyInfo Proxy { get; }
      public int BadRequestCount { get; set; }
      public int GoodRequestCount { get; set; }

      public ProxyStatistics(ProxyInfo proxy)
      {
        Proxy = proxy;
        BadRequestCount = 0;
        GoodRequestCount = 0;
      }
    }
  }
}