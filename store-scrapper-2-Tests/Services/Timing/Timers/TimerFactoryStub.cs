using System;
using store_scrapper_2.Services;

namespace store_scrapper_2_Tests.Services.Timing.Timers
{
  public class TimerFactoryStub : ITimerFactory
  {
    public int CreateCount { get; private set; }
    public TimerStub Timer { get; private set; } = new TimerStub(() => {}, 1000);
    
    public ITimer Create(Action timerElapsedDelegate, uint intervalMs)
    {
      CreateCount++;
      return Timer = new TimerStub(timerElapsedDelegate, intervalMs);
    }

    public ITimer LastCreated => Timer;
  }
}