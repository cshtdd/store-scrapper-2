using System;
using store_scrapper_2.Services;

namespace store_scrapper_2_Tests.Services.Timing.Timers
{
  public class TimerStub : ITimer
  {
    private readonly Action _elapsedEventHandler;
    public uint IntervalMs { get; }
    
    public int StopCallCount { get; set; }
    public int StartCallCount { get; set; }
    
    public TimerStub(Action elapsedEventHandler, uint intervalMs)
    {
      _elapsedEventHandler = elapsedEventHandler;
      IntervalMs = intervalMs;
    }

    public void InvokeElapsed() => _elapsedEventHandler();

    public void Dispose() => Stop();
    public void Start() => StartCallCount++;
    public void Stop() => StopCallCount++;
  }
}