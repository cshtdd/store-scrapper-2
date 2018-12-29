using System.Threading;
using FluentAssertions;
using store_scrapper_2.Services;
using Xunit;

namespace store_scrapper_2_Tests.Services.Timing.Timers
{
  public class TimerFactoryTest
  {
    private readonly TimerFactory factory = new TimerFactory();

    [Fact]
    public void LastCreatedAlwaysReturnSomething()
    {
      factory.LastCreated.Should().NotBeNull();
      factory.LastCreated.Should().Be(factory.LastCreated);
      factory.LastCreated.Start();
      factory.LastCreated.Stop();
    }

    [Fact]
    public void CreatesTimersThatWork()
    {
      int elapsedCount = 0;

      factory.Create(() => { elapsedCount++; }, 1);
      elapsedCount.Should().Be(0);

      Thread.Sleep(10);
      elapsedCount.Should().Be(0);
      
      factory.LastCreated.Start();
      Thread.Sleep(10);
      elapsedCount.Should().BeGreaterThan(3);

      factory.LastCreated.Stop();
      int currentElapsedCount = elapsedCount;

      Thread.Sleep(10);
      currentElapsedCount.Should().Be(elapsedCount);
    }
  }
}