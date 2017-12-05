using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2;
using store_scrapper_2_int_Tests.Utils;
using Xunit;

namespace store_scrapper_2_int_Tests.DAL
{
  public class PersistenceInitializerTest : DatabaseTest
  {
    [Fact]
    public async Task RunsTheMigrations()
    {
      using (var context = ContextFactory.Create())
      {
        context.Database.GetPendingMigrations().Should().NotBeEmpty();
      }
      DatabaseExists.Should().BeFalse();
      
      await new PersistenceInitializer(ContextFactory).InitializeAsync();

      using (var context = ContextFactory.Create())
      {
        context.Database.GetPendingMigrations().Should().BeEmpty();
      }
      DatabaseExists.Should().BeTrue();
    }
  }
}