using System;
using store_scrapper_2.Configuration;
using store_scrapper_2.Logging;

namespace store_scrapper_2.Services.Timing
{
  public class DeadlockDetector : IDeadlockDetector, IDisposable
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
    private readonly IConfigurationReader _configurationReader;
    private readonly ITimerFactory _timerFactory;

    private bool _initialized;
    private readonly object _statusLock = new object();
    private DateTime _lastStatus;

    public DeadlockDetector(IConfigurationReader configurationReader, ITimerFactory timerFactory)
    {
      _configurationReader = configurationReader;
      _timerFactory = timerFactory;
    }

    private bool IsEnabled => _configurationReader.ReadBool(ConfigurationKeys.DeadlockDetectionEnabled);
    private uint TimeoutMs => _configurationReader.ReadUInt(ConfigurationKeys.DeadlockDetectionTimeoutMs, 600000);

    public void UpdateStatus()
    {
      Init();
      
      lock (_statusLock)
      {
        _lastStatus = DateTime.UtcNow;        
      }
    }
    
    private void Init()
    {
      if (_initialized)
      {
        return;
      }
      _initialized = true;
      
      Logger.LogInfo("Init", nameof(IsEnabled), IsEnabled, nameof(TimeoutMs), TimeoutMs);
      UpdateStatus();
      StartTimer();
    }

    public void Dispose() => StopTimer();

    private void StartTimer()
    {
      if (!IsEnabled)
      {
        return;
      }

      _timerFactory.Create(CheckDeadlock, 1000).Start();
    }

    private void StopTimer()
    {
      if (!IsEnabled)
      {
        return;
      }

      _timerFactory.LastCreated.Stop();
    }

    private void CheckDeadlock()
    {
      double inactivityPeriodMs;
      
      lock (_statusLock)
      {
        inactivityPeriodMs = (DateTime.UtcNow - _lastStatus).TotalMilliseconds;
      }

      if (inactivityPeriodMs > TimeoutMs)
      {
        Logger.LogError("Deadlock Detected", nameof(TimeoutMs), TimeoutMs, nameof(inactivityPeriodMs), inactivityPeriodMs);
        AbortProgram();
      }
    }

    private void AbortProgram()
    {
      System.Threading.ThreadPool.QueueUserWorkItem(_ =>
      {
        throw new InvalidOperationException("Deadlock Detected");
      });
    }
  }
}