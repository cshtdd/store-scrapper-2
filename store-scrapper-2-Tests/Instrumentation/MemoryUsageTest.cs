using FluentAssertions;
using store_scrapper_2.Instrumentation;
using store_scrapper_2_Tests.Services.Timing.Timers;
using Xunit;

namespace store_scrapper_2_Tests.Instrumentation
{
  public class MemoryUsageTest
  {
    private readonly TimerFactoryStub _timerFactory = new TimerFactoryStub();
    private readonly MemoryUsage _memoryUsage;

    public MemoryUsageTest()
    {
      _memoryUsage = new MemoryUsage(_timerFactory);
    }

    [Fact]
    public void StartCreatesAndStartsATimer()
    {
      _memoryUsage.Start();

      _timerFactory.CreateCount.Should().Be(1);
      _timerFactory.Timer.IntervalMs.Should().Be(1000);
      _timerFactory.Timer.StartCallCount.Should().Be(1);
      _timerFactory.Timer.StopCallCount.Should().Be(0);
    }

    [Fact]
    public void StopShutdownsTheTimer()
    {
      _memoryUsage.Stop();
      
      _timerFactory.CreateCount.Should().Be(0);
      _timerFactory.Timer.StartCallCount.Should().Be(0);
      _timerFactory.Timer.StopCallCount.Should().Be(1);
    }
  }
}