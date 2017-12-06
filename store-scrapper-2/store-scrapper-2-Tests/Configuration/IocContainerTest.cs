using FluentAssertions;
using store_scrapper_2;
using store_scrapper_2.Configuration;
using store_scrapper_2.DAL;
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
      IocContainer.Resolve<SingleStoreProcessor>().Should().NotBeNull();
      IocContainer.Resolve<IPersistenceInitializer>().Should().NotBeNull();
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