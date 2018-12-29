using System;

namespace store_scrapper_2.Services
{
  internal sealed class Timer : ITimer
  {
    private readonly Action _timerElapsedDelegate;
    private readonly uint _intervalMs;
    
    private bool _disposed;
    private System.Timers.Timer _timer;

    internal Timer(Action timerElapsedDelegate, uint intervalMs)
    {
      _timerElapsedDelegate = timerElapsedDelegate;
      _intervalMs = intervalMs;
    }

    private void TimerElapsedEventHandler(object sender, EventArgs e) => _timerElapsedDelegate();

    public void Start()
    {
      if (_timer != null)
      {
        return;
      }
      
      _timer = new System.Timers.Timer();
      _timer.Interval = _intervalMs;
      _timer.Elapsed += TimerElapsedEventHandler;
      _timer.Start();
    }

    public void Stop()
    {
      if (_timer == null)
      {
        return;
      }
      
      _timer.Elapsed -= TimerElapsedEventHandler;
      _timer.Stop();
      _timer.Dispose();
      _timer = null;
    }

    public void Dispose()
    {      
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
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
  }
}