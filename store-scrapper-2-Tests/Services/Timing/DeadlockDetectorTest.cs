using System.Threading;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2.Configuration;
using store_scrapper_2_Tests.Services.Timing.Timers;
using Xunit;

namespace store_scrapper_2_Tests.Services.Timing
{
  public class DeadlockDetectorTest
  {
    private readonly IConfigurationReader _configurationReader = Substitute.For<IConfigurationReader>();
    private readonly TimerFactoryStub _timerFactory = new TimerFactoryStub();

    private readonly DeadlockDetectorStub _deadlockDetector;

    public DeadlockDetectorTest()
    {
      _deadlockDetector = new DeadlockDetectorStub(_configurationReader, _timerFactory);
    }

    [Fact]
    public void AbortsTheProgramWhenTimeoutExpired()
    {
      SetupDeadlockTimeoutMs(5);
      EnableDeadlockDetector();
      
      _deadlockDetector.UpdateStatus();
      _timerFactory.CreateCount.Should().Be(1);
      _timerFactory.Timer.StartCallCount.Should().Be(1);
      _timerFactory.Timer.StopCallCount.Should().Be(0);

      _deadlockDetector.UpdateStatus();
      _timerFactory.CreateCount.Should().Be(1);
      _timerFactory.Timer.StartCallCount.Should().Be(1);
      _timerFactory.Timer.StopCallCount.Should().Be(0);
      
      Thread.Sleep(10);
      _timerFactory.Timer.InvokeElapsed();
      
      _deadlockDetector.ProgramAborted.Should().BeTrue();
    }

    [Fact]
    public void MultipleStatusUpdatesDoNotStartMultipleTimers()
    {
      SetupDeadlockTimeoutMs(5);
      EnableDeadlockDetector();
      
      _deadlockDetector.UpdateStatus();
      _deadlockDetector.UpdateStatus();
      _deadlockDetector.UpdateStatus();
      _deadlockDetector.UpdateStatus();
      
      _timerFactory.CreateCount.Should().Be(1);
      _timerFactory.Timer.IntervalMs.Should().Be(1000);
      _timerFactory.Timer.StartCallCount.Should().Be(1);
      _timerFactory.Timer.StopCallCount.Should().Be(0);
    }

    [Fact]
    public void DisposingTheDeadlockDetectorStopsTheTimer()
    {
      SetupDeadlockTimeoutMs(5);
      EnableDeadlockDetector();

      _deadlockDetector.Dispose();
      
      _timerFactory.CreateCount.Should().Be(0);
      _timerFactory.Timer.StartCallCount.Should().Be(0);
      _timerFactory.Timer.StopCallCount.Should().Be(1);
    }
    
    [Fact]
    public void DoesNotAbortTheProgramWhenTimeoutHasNotExpired()
    {
      SetupDeadlockTimeoutMs(5000000);
      EnableDeadlockDetector();
      
      _deadlockDetector.UpdateStatus();
      _timerFactory.CreateCount.Should().Be(1);
      _timerFactory.Timer.StartCallCount.Should().Be(1);
      _timerFactory.Timer.StopCallCount.Should().Be(0);

      Thread.Sleep(10);
      _timerFactory.Timer.InvokeElapsed();

      _deadlockDetector.ProgramAborted.Should().BeFalse();
    }
    
    [Fact]
    public void DoesNotStartTheTimerWhenDisableInTheConfiguration()
    {
      SetupDeadlockTimeoutMs(5);
      DisableDeadlockDetector();
      
      _deadlockDetector.UpdateStatus();

      _timerFactory.CreateCount.Should().Be(0);
      _timerFactory.Timer.StartCallCount.Should().Be(0);
      _timerFactory.Timer.StopCallCount.Should().Be(0);
    }
    
    [Fact]
    public void DoesNotStopTheTimerWhenDisableInTheConfiguration()
    {
      SetupDeadlockTimeoutMs(5);
      DisableDeadlockDetector();
      
      _deadlockDetector.Dispose();

      _timerFactory.CreateCount.Should().Be(0);
      _timerFactory.Timer.StartCallCount.Should().Be(0);
      _timerFactory.Timer.StopCallCount.Should().Be(0);
    }

    private void SetupDeadlockTimeoutMs(uint value) =>
      _configurationReader.ReadUInt(ConfigurationKeys.DeadlockDetectionTimeoutMs, 600000).Returns(value);

    private void EnableDeadlockDetector() =>
      _configurationReader.ReadBool(ConfigurationKeys.DeadlockDetectionEnabled).Returns(true);
    
    private void DisableDeadlockDetector() =>
      _configurationReader.ReadBool(ConfigurationKeys.DeadlockDetectionEnabled).Returns(false);
  }
}