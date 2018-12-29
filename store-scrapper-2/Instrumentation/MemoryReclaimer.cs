using System;
using System.Diagnostics;
using System.Runtime;
using store_scrapper_2.Logging;
using store_scrapper_2.Services;

namespace store_scrapper_2.Instrumentation
{
  public class MemoryReclaimer : IPerformanceCounter
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
    private readonly ITimerFactory _timerFactory;
    private readonly object _statusLock = new object();
    private bool _gcBusy = false;
    
    public MemoryReclaimer(ITimerFactory timerFactory)
    {
      _timerFactory = timerFactory;
    }

    public void Start() => _timerFactory.Create(InvokeGarbageCollection, 60000).Start();
    public void Stop() => _timerFactory.LastCreated.Stop();

    private void InvokeGarbageCollection()
    {
      var stopwatch = Stopwatch.StartNew();
      var shouldCollect = AcquireCollectionLock();
      
      Logger.LogInfo("InvokeGarbageCollectionStart", "ShouldCollect", shouldCollect);
      
      if (shouldCollect)
      {
        CollectInternal();        
      }
      
      stopwatch.Stop();
      Logger.LogInfo("InvokeGarbageCollection", 
        "Result", true,
        "ShouldCollect", shouldCollect,
        "ElapsedMs", stopwatch.ElapsedMilliseconds
        );
    }

    private void CollectInternal()
    {
      try
      {
        var prevCompactionMode = GCSettings.LargeObjectHeapCompactionMode;

        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        GC.Collect();

        GCSettings.LargeObjectHeapCompactionMode = prevCompactionMode;
      }
      finally
      {
        ReleaseCollectionLock();
      }
    }

    private bool AcquireCollectionLock()
    {
      var result = false;
      
      lock (_statusLock)
      {
        if (!_gcBusy)
        {
          _gcBusy = true;
          result = true;
        }
      }

      return result;
    }
    
    private void ReleaseCollectionLock()
    {
      lock (_statusLock)
      {
        _gcBusy = false;
      }
    }
  }
}