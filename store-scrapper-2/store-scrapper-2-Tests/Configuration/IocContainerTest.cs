using FluentAssertions;
using store_scrapper_2;
using store_scrapper_2.Configuration;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DAL;
using store_scrapper_2.Services;
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
      IocContainer.Resolve<IMultipleZipCodeProcessor>().Should().NotBeNull();
      IocContainer.Resolve<ISingleZipCodeProcessor>().Should().NotBeNull();
      IocContainer.Resolve<IPersistenceInitializer>().Should().NotBeNull();
      IocContainer.Resolve<IZipCodeUrlSerializer>().Should().NotBeNull();      
      IocContainer.Resolve<IStoreInfoResponseDataService>().Should().NotBeNull();
      IocContainer.Resolve<IZipCodeDataService>().Should().NotBeNull();
      IocContainer.Resolve<IZipCodeBatchesReader>().Should().NotBeNull();
      IocContainer.Resolve<IAllZipCodesProcessor>().Should().NotBeNull();
    }

    [Fact]
    public void CorrectlyResolvesTheConnectionConfigurationReader()
    {
      ((ConfigurationReader) IocContainer.Resolve<IConfigurationReader>()).EnvironmentName.Should().Be("PROD");
    }

    [Fact]
    public void CorrectlyBuildsStoreDataContextFactories()
    {
      var storeDataContextFactory = IocContainer.Resolve<IStoreDataContextFactory>();
      
      storeDataContextFactory.Should().NotBeNull();
      
      ((StoreDataContextFactory) storeDataContextFactory).ConnectionString.Should().Contain("Server=localhost");
      ((StoreDataContextFactory) storeDataContextFactory).ConnectionString.Should().Contain("Database=stores");
    }
  }
}