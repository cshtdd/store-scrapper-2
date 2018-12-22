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
    private readonly List<ProxyStatistics> _proxyStorage = new List<ProxyStatistics>();
    
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    
    private readonly IProxyListReader _proxyListReader;
    private readonly IProxyReadingStrategy _proxyReadingStrategy;
    private readonly IConfigurationReader _configurationReader;

    public ProxyRepository(IProxyListReader proxyListReader, IProxyReadingStrategy proxyReadingStrategy, IConfigurationReader configurationReader)
    {
      _proxyListReader = proxyListReader;
      _proxyReadingStrategy = proxyReadingStrategy;
      _configurationReader = configurationReader;
    }

    public ProxyInfo Read()
    {
      if (HasNoProxies())
      {
        ReadProxies();
      }

      return _proxyReadingStrategy.Read(_proxyStorage);
    }

    private void ReadProxies()
    {
      _proxyReadingStrategy.Reset();
      var toProxyStatistics = ToProxyStatistics();
      var newProxies = _proxyListReader.Read().Select(toProxyStatistics);
      _proxyStorage.AddRange(newProxies);
      
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

    private bool HasNoProxies() => _proxyStorage.Count == 0;
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

    private uint ReadFailedThreshold() => _configurationReader.ReadUInt(ConfigurationKeys.ProxyFailThreshold, 5);
    private uint ReadSuccessThreshold() => _configurationReader.ReadUInt(ConfigurationKeys.ProxyMaxCount, 100);
    
    private ProxyStatistics Find(ProxyInfo proxy)
    {
      var result = _proxyStorage.Find(s => s.Proxy.Equals(proxy));

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
        _proxyStorage.Remove(statistics);
        
        Logger.LogInfo("RemoveOverUsedProxy", 
          "Proxy", statistics.Proxy,
          "SuccessCount", statistics.SuccessCount,
          "SuccessThreshold", statistics.SuccessThreshold,
          "FailedCount", statistics.FailedCount,
          "FailedThreshold", statistics.FailedThreshold
        );
      }
    }     
  }
}