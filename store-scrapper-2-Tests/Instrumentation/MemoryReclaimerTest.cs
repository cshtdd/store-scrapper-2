using FluentAssertions;
using store_scrapper_2.Instrumentation;
using store_scrapper_2_Tests.Services.Timing.Timers;
using Xunit;

namespace store_scrapper_2_Tests.Instrumentation
{
  public class MemoryReclaimerTest
  {
    private readonly TimerFactoryStub _timerFactory = new TimerFactoryStub();

    private readonly MemoryReclaimer _memoryReclaimer;

    public MemoryReclaimerTest()
    {
      _memoryReclaimer = new MemoryReclaimer(_timerFactory);
    }

    [Fact]
    public void StartCreatesAndStartsATimer()
    {
      _memoryReclaimer.Start();

      _timerFactory.CreateCount.Should().Be(1);
      _timerFactory.Timer.IntervalMs.Should().Be(60000);
      _timerFactory.Timer.StartCallCount.Should().Be(1);
      _timerFactory.Timer.StopCallCount.Should().Be(0);
    }

    [Fact]
    public void StopShutdownsTheTimer()
    {
      _memoryReclaimer.Stop();
      
      _timerFactory.CreateCount.Should().Be(0);
      _timerFactory.Timer.StartCallCount.Should().Be(0);
      _timerFactory.Timer.StopCallCount.Should().Be(1);
    }
  }
}