using System.Linq;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2.Configuration;
using store_scrapper_2.Instrumentation;
using Xunit;

namespace store_scrapper_2_Tests.Instrumentation
{
  public class ResourcesManagerTest
  {
    private readonly IConfigurationReader _configurationReader = Substitute.For<IConfigurationReader>();

    [Fact]
    public void ReceivesAListOfPerformanceCounters()
    {
      var manager = new ResourcesManager(new[]
      {
        Substitute.For<IPerformanceCounter>(),
        Substitute.For<IPerformanceCounter>()
      }, _configurationReader);

      manager.Counters.Count().Should().Be(2);
    }

    [Fact]
    public void MonitorStartsAllThePerformanceCounters()
    {
      EnableInstrumentation();
      var counter1 = Substitute.For<IPerformanceCounter>();
      var counter2 = Substitute.For<IPerformanceCounter>();
      var resourcesManager = new ResourcesManager(new[] { counter1, counter2 }, _configurationReader);
      
      resourcesManager.Monitor();
      
      counter1.Received().Start();
      counter2.Received().Start();
    }
    
    [Fact]
    public void MonitorDoesNotDoAnythingWhenInstrumentationDisabled()
    {
      DisableInstrumentation();
      var counter1 = Substitute.For<IPerformanceCounter>();
      var counter2 = Substitute.For<IPerformanceCounter>();
      var resourcesManager = new ResourcesManager(new[] { counter1, counter2 }, _configurationReader);
      
      resourcesManager.Monitor();
      
      counter1.DidNotReceive().Start();
      counter2.DidNotReceive().Start();
    }

    [Fact]
    public void DisposeStopsAllThePerformanceCounters()
    {
      EnableInstrumentation();
      var counter1 = Substitute.For<IPerformanceCounter>();
      var counter2 = Substitute.For<IPerformanceCounter>();
      var resourcesManager = new ResourcesManager(new[] { counter1, counter2 }, _configurationReader);
      
      resourcesManager.Dispose();
      
      counter1.Received().Stop();
      counter2.Received().Stop();
    }

    [Fact]
    public void DisposeDoesNotDoAnythingWhenInstrumentationDisabled()
    {
      DisableInstrumentation();
      var counter1 = Substitute.For<IPerformanceCounter>();
      var counter2 = Substitute.For<IPerformanceCounter>();
      var resourcesManager = new ResourcesManager(new[] { counter1, counter2 }, _configurationReader);
      
      resourcesManager.Dispose();
      
      counter1.DidNotReceive().Stop();
      counter2.DidNotReceive().Stop();
    }
    
    private void EnableInstrumentation()
    {
      _configurationReader
        .ReadBool(ConfigurationKeys.InstrumentationEnabled)
        .Returns(true);
    }
    
    private void DisableInstrumentation()
    {
      _configurationReader
        .ReadBool(ConfigurationKeys.InstrumentationEnabled)
        .Returns(false);
    }
  }
}