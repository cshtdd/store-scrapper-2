using System;
using System.Collections.Generic;
using store_scrapper_2.Configuration;

namespace store_scrapper_2.Instrumentation
{
  public class ResourcesManager : IResourcesManager
  {
    private bool _disposed;

    private readonly IConfigurationReader _configurationReader;
    private readonly List<IPerformanceCounter> _counters = new List<IPerformanceCounter>();

    public ResourcesManager(IEnumerable<IPerformanceCounter> counters, IConfigurationReader configurationReader)
    {
      _configurationReader = configurationReader;
      _counters.AddRange(counters);
    }

    private bool Enabled => _configurationReader.ReadBool(ConfigurationKeys.InstrumentationEnabled);
    public IEnumerable<IPerformanceCounter> Counters => _counters.ToArray();
    
    public void Monitor()
    {
      Start();
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
      if (_disposed)
      {
        return;       
      }

      if (disposing)
      {
        Stop();
      }

      _disposed = true;
    }

    private void Start()
    {
      if (!Enabled)
      {
        return;
      }

      _counters.ForEach(c => c.Start());
    }
    
    private void Stop()
    {
      if (!Enabled)
      {
        return;
      }
      
      _counters.ForEach(c => c.Stop());
    }
  }
}