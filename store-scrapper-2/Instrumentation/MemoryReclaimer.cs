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

    public MemoryReclaimer(ITimerFactory timerFactory)
    {
      _timerFactory = timerFactory;
    }

    public void Start() => _timerFactory.Create(InvokeGarbageCollection, 60000).Start();
    public void Stop() => _timerFactory.LastCreated.Stop();

    private void InvokeGarbageCollection()
    {
      var stopwatch = Stopwatch.StartNew();

      GCCollectInternal();

      stopwatch.Stop();
      Logger.LogInfo("InvokeGarbageCollection", "ElapsedMs", stopwatch.ElapsedMilliseconds);
    }

    private static void GCCollectInternal()
    {
      var prevCompactionMode = GCSettings.LargeObjectHeapCompactionMode;
      
      GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
      GC.Collect();
      
      GCSettings.LargeObjectHeapCompactionMode = prevCompactionMode;
    }
  }
}