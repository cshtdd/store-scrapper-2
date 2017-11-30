using FluentAssertions;
using store_scrapper_2;
using store_scrapper_2.Configuration;
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
    }
  }
}