using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using store_scrapper_2.Configuration;
using store_scrapper_2.Logging;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyRepository : IProxyRepository
  {
    private int lastReadIndex;
    private readonly List<ProxyStatistics> proxyStorage = new List<ProxyStatistics>();
    
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    
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
        ReadProxies();
      }

      return proxyStorage[lastReadIndex++ % proxyStorage.Count].Proxy;
    }

    private void ReadProxies()
    {
      lastReadIndex = 0;
      var toProxyStatistics = ToProxyStatistics();
      var newProxies = _proxyListReader.Read().Select(toProxyStatistics);
      proxyStorage.AddRange(newProxies);
      
      Logger.LogInfo("ReadProxies", "Count", newProxies.Count());
    }

    public void CountSuccessRequest(ProxyInfo proxy)
    {
      EnsureProxiesHaveBeenRead();

      var statistics = Find(proxy).IncrementSuccessCount();
      RemoveIfNeeded(statistics);
    }

    public void CountFailedRequest(ProxyInfo proxy)
    {
      EnsureProxiesHaveBeenRead();

      var statistics = Find(proxy).IncrementFailedCount();
      RemoveIfNeeded(statistics);
    }

    private bool HasNoProxies() => proxyStorage.Count == 0;
    private void EnsureProxiesHaveBeenRead()
    {
      if (HasNoProxies())
      {
        throw new InvalidOperationException("No Proxy has been read");       
      }
    }

    private Func<ProxyInfo, ProxyStatistics> ToProxyStatistics()
    {
      var successThreshold = ReadSuccessThreshold();
      var failThreshold = ReadFailedThreshold();

      return proxyInfo => new ProxyStatistics(proxyInfo, successThreshold, failThreshold);
    }

    private int ReadFailedThreshold() => _configurationReader.ReadInt(ConfigurationKeys.ProxyFailThreshold, 5);
    private int ReadSuccessThreshold() => _configurationReader.ReadInt(ConfigurationKeys.ProxyMaxCount, 100);
    
    private ProxyStatistics Find(ProxyInfo proxy)
    {
      var result = proxyStorage.Find(s => s.Proxy.Equals(proxy));

      if (result == null)
      {
        throw new InvalidOperationException($"Cannot find Proxy:{proxy}");
      }
      
      return result;
    }

    private void RemoveIfNeeded(ProxyStatistics statistics)
    {
      if (statistics.HasBeenUsedTooMuch)
      {
        proxyStorage.Remove(statistics);
        
        Logger.LogInfo("RemoveOverUsedProxy", 
          "Proxy", statistics.Proxy,
          "SuccessCount", statistics.SuccessCount,
          "SuccessThreshold", statistics.SuccessThreshold,
          "FailedCount", statistics.FailedCount,
          "FailedThreshold", statistics.FailedThreshold
        );
      }
    } 
    
    private class ProxyStatistics
    {
      public ProxyInfo Proxy { get; }
      
      public int SuccessThreshold { get; }
      public int FailedThreshold { get; }

      public int SuccessCount { get; set; }
      public int FailedCount { get; set; }

      public ProxyStatistics(ProxyInfo proxy, int successThreshold, int failedThreshold)
      {
        Proxy = proxy;

        SuccessCount = 0;
        SuccessThreshold = successThreshold;

        FailedCount = 0;
        FailedThreshold = failedThreshold;
      }

      public bool HasBeenUsedTooMuch => SuccessCount >= SuccessThreshold ||
                                        FailedCount >= FailedThreshold;

      public ProxyStatistics IncrementFailedCount()
      {
        FailedCount++;
        return this;
      }
      
      public ProxyStatistics IncrementSuccessCount()
      {
        SuccessCount++;
        return this;
      }
    }
  }
}