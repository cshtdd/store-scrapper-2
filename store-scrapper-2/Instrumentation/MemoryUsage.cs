using System;
using System.Diagnostics;
using System.Timers;
using store_scrapper_2.Logging;

namespace store_scrapper_2.Instrumentation
{
  public class MemoryUsage : IPerformanceCounter
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
    private Timer _memoryTimer;
    
    public void Start()
    {
      if (_memoryTimer != null)
      {
        return;
      }
      
      _memoryTimer = new Timer();
      _memoryTimer.Interval = 1000;
      _memoryTimer.Elapsed += CheckMemoryUsage;
      _memoryTimer.Start();
    }

    public void Stop()
    {
      if (_memoryTimer == null)
      {
        return;
      }
      
      _memoryTimer.Elapsed -= CheckMemoryUsage;
      _memoryTimer.Stop();
      _memoryTimer.Dispose();
      _memoryTimer = null;
    }

    private void CheckMemoryUsage(object sender, EventArgs e)
    {
      var currentProcess = Process.GetCurrentProcess();
      Logger.LogInfo("MemoryUsage", 
        "Responding", currentProcess.Responding,
        "MinWorkingSet", currentProcess.MinWorkingSet,
        "WorkingSet64", currentProcess.WorkingSet64,
        "MaxWorkingSet", currentProcess.MaxWorkingSet,
        "PagedMemorySize64", currentProcess.PagedMemorySize64
        );
    }
  }
}