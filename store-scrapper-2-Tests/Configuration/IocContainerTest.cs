using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using store_scrapper_2;
using store_scrapper_2.Configuration;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DataTransmission.Web;
using store_scrapper_2.DataTransmission.Web.Proxy;
using store_scrapper_2.DAL;
using store_scrapper_2.Instrumentation;
using store_scrapper_2.Services;
using store_scrapper_2.Services.Timing;
using Xunit;

namespace store_scrapper_2_Tests.Configuration
{
  public class IocContainerTest
  {
    public IocContainerTest()
    {
      IocContainer.Initialize();     
    }
    
    [Fact]
    public void CanResolve()
    {
      IocContainer.Resolve<ISingleZipCodeProcessor>().Should().NotBeNull();
      IocContainer.Resolve<IPersistenceInitializer>().Should().NotBeNull();
      IocContainer.Resolve<IZipCodeUrlSerializer>().Should().NotBeNull();      
      IocContainer.Resolve<IStoreInfoResponseDataService>().Should().NotBeNull();
      IocContainer.Resolve<IZipCodeDataService>().Should().NotBeNull();
      IocContainer.Resolve<IAllZipCodesProcessor>().Should().NotBeNull();
      IocContainer.Resolve<IDelaySimulator>().Should().NotBeNull();
      IocContainer.Resolve<ICacheWithExpiration>().Should().NotBeNull();
      IocContainer.Resolve<IStorePersistenceCalculator>().Should().NotBeNull();
      IocContainer.Resolve<ITimerFactory>().Should().NotBeNull();

      ValidateSingletonRegistration<IMemoryCache>();
    }
    
    [Fact]
    public void CorrectlyResolvesTheUrlDownloader()
    {
      (IocContainer.Resolve<IUrlDownloader>() as UrlDownloader).Should().NotBeNull();
    }
    
    [Fact]
    public void CorrectlyResolvesTheProxyReadingStrategy()
    {
      (IocContainer.Resolve<IProxyReadingStrategy>() as ProxyReadingStrategyGreedy).Should().NotBeNull();
    }

    [Fact]
    public void CorrectlyResolvesTheProxyRoundRobinUrlDownloader()
    {
      (IocContainer.Resolve<IProxyRoundRobinUrlDownloader>() as ProxyRoundRobinUrlDownloader).Should().NotBeNull();      
      ValidateSingletonRegistration<IProxyRoundRobinUrlDownloader>();
    }
    
    [Fact]
    public void CorrectlyResolvesTheDeadlockDetector()
    {
      (IocContainer.Resolve<IDeadlockDetector>() as DeadlockDetector).Should().NotBeNull();      
      ValidateSingletonRegistration<IDeadlockDetector>();
    }

    [Fact]
    public void CorrectlyResolvesTheResourcesManager()
    {
      (IocContainer.Resolve<IResourcesManager>() as ResourcesManager).Should().NotBeNull();
      ValidateSingletonRegistration<IResourcesManager>();

      var resourcesManager = (ResourcesManager)IocContainer.Resolve<IResourcesManager>();

      resourcesManager
        .Counters
        .Count()
        .Should()
        .Be(3);
      
      resourcesManager
        .Counters
        .FirstOrDefault(c => c is MemoryUsage)
        .Should()
        .NotBeNull();
      
      resourcesManager
        .Counters
        .FirstOrDefault(c => c is UnhandledExceptionLogger)
        .Should()
        .NotBeNull();
      
      resourcesManager
        .Counters
        .FirstOrDefault(c => c is MemoryReclaimer)
        .Should()
        .NotBeNull();
    }
    
    [Fact]
    public void CorrectlyResolvesTheStoreInfoDownloader()
    {
      var storeInfoDownloader = IocContainer.Resolve<IStoreInfoDownloader>() as StoreInfoDownloader;

      storeInfoDownloader.Should().NotBeNull();

      (storeInfoDownloader._urlDownloader as ProxyRoundRobinUrlDownloader).Should().NotBeNull();
    }

    [Fact]
    public void CorrectlyResolvesTheConnectionConfigurationReader()
    {
      ((ConfigurationReader) IocContainer.Resolve<IConfigurationReader>()).EnvironmentName.Should().Be("PROD");
    }

    [Fact]
    public void CorrectlyResolvesTheMemoryCacheAsMemoryCache()
    {
      (IocContainer.Resolve<IMemoryCache>() as MemoryCache).Should().NotBeNull();
    }

    [Fact]
    public void CorrectlyResolvesTheWebExceptionHandler()
    {
      (IocContainer.Resolve<IWebExceptionHandler>() as IgnorePaymentRequiredExceptions).Should().NotBeNull();
    }
    
    [Fact]
    public void CorrectlyBuildsStoreDataContextFactories()
    {
      var storeDataContextFactory = IocContainer.Resolve<IStoreDataContextFactory>();
      
      storeDataContextFactory.Should().NotBeNull();
      
      ((StoreDataContextFactory) storeDataContextFactory).ConnectionString.Contains("Server=localhost").Should().BeTrue();
      ((StoreDataContextFactory) storeDataContextFactory).ConnectionString.Contains("Database=stores").Should().BeTrue();
    }

    private static void ValidateSingletonRegistration<T>()  
    {
      var a = IocContainer.Resolve<T>();
      var b = IocContainer.Resolve<T>();
      a.Equals(b).Should().BeTrue();
    }
  }
}