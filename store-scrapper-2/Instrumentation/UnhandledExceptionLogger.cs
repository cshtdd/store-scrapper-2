using System;
using store_scrapper_2.Logging;

namespace store_scrapper_2.Instrumentation
{
  public class UnhandledExceptionLogger : IPerformanceCounter
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
    public void Start() => AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;
    public void Stop() { }

    private void LogUnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
      Logger.LogError("App Domain Unhandled Exception",
        args.ExceptionObject,
        "IsTerminating", args.IsTerminating);
    }
  }
}