using System;
using store_scrapper_2.Configuration;
using store_scrapper_2.Logging;

namespace store_scrapper_2.Services
{
  public class DeadlockDetector : IDeadlockDetector, IDisposable
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
    private readonly IConfigurationReader _configurationReader;

    private bool _initialized;
    private readonly object _statusLock = new object();
    private bool _disposed;
    private readonly System.Timers.Timer _deadlockTimer = new System.Timers.Timer();
    private DateTime _lastStatus;

    public DeadlockDetector(IConfigurationReader configurationReader)
    {
      _configurationReader = configurationReader;
      _deadlockTimer.Interval = 1000;
      _deadlockTimer.Enabled = false;
      _deadlockTimer.Elapsed += CheckDeadlock;
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
        StopTimer();
        _deadlockTimer.Elapsed -= CheckDeadlock;
        _deadlockTimer.Dispose();
      }

      _disposed = true;
    }

    private void StartTimer()
    {
      if (!IsEnabled)
      {
        return;
      }

      _deadlockTimer.Start();
    }

    private void StopTimer()
    {
      if (!IsEnabled)
      {
        return;
      }

      _deadlockTimer.Stop();
    }

    private void CheckDeadlock(object sender, EventArgs e)
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