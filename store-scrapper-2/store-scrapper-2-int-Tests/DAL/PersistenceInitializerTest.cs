using System.Linq;
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

      await CreatePersistenceInitializer().InitializeAsync();

      using (var context = ContextFactory.Create())
      {
        context.Database.GetPendingMigrations().Should().BeEmpty();
      }
    }

    [Fact]
    public async Task SeedsTheZipCodes()
    {
      await CreatePersistenceInitializer().InitializeAsync();

      using (var context = ContextFactory.Create())
      {
        var zipCodesCount = await context.Zips.CountAsync();
        zipCodesCount.Should().BeInRange(40000, 45000);

        var uniqueZipCodesCount = await context.Zips
          .Select(_ => _.ZipCode)
          .Distinct()
          .CountAsync();
        uniqueZipCodesCount.Should().Be(zipCodesCount);
      }
    }
  }
}