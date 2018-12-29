using System;

namespace store_scrapper_2.Services
{
  public class TimerFactory : ITimerFactory
  {
    public ITimer LastCreated { get; private set; } = new NoTimer();

    public ITimer Create(Action timerElapsedDelegate, uint intervalMs) => 
      LastCreated = new Timer(timerElapsedDelegate, intervalMs);

    private class NoTimer : ITimer
    {
      public void Dispose() {}
      public void Start() {}
      public void Stop() {}
    }
  }
}