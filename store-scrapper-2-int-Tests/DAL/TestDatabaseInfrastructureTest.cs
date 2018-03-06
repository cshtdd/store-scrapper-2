using FluentAssertions;
using store_scrapper_2.DAL;
using store_scrapper_2_int_Tests.Utils;
using Xunit;

namespace store_scrapper_2_int_Tests.DAL
{
  public class TestDatabaseInfrastructureTest : DatabaseTest
  {
    [Fact]
    public void UsesATestDatabase()
    {
      ((StoreDataContextFactory) ContextFactory).ConnectionString.Should().Contain("Server=localhost");
      ((StoreDataContextFactory) ContextFactory).ConnectionString.Should().Contain("Database=test_stores"); 
    }
  }
}