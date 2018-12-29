using System;
using System.Diagnostics;
using store_scrapper_2.Logging;
using store_scrapper_2.Services;

namespace store_scrapper_2.Instrumentation
{
  public class MemoryUsage : IPerformanceCounter
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly ITimerFactory _timerFactory;

    public MemoryUsage(ITimerFactory timerFactory) => _timerFactory = timerFactory;

    public void Start() => _timerFactory.Create(CheckMemoryUsage, 1000).Start();

    public void Stop() => _timerFactory.LastCreated.Stop();

    private void CheckMemoryUsage()
    {
      var currentProcess = Process.GetCurrentProcess();
      Logger.LogInfo("MemoryUsage", 
        "Responding", currentProcess.Responding,
        "MinWorkingSet", currentProcess.MinWorkingSet,
        "WorkingSet64", currentProcess.WorkingSet64,
        "MaxWorkingSet", currentProcess.MaxWorkingSet,
        "PagedMemorySize64", currentProcess.PagedMemorySize64,  
        "WorkingSet64KB", Convert.ToInt32(currentProcess.WorkingSet64 / 1024),
        "TotalGCMemoryKB", Convert.ToInt32(GC.GetTotalMemory(false) / 1024)
      );
    }
  }
}