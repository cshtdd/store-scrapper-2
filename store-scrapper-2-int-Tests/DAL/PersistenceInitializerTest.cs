using System.Linq;

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
    public void RunsTheMigrations()
    {
      using (var context = ContextFactory.Create())
      {
        context.Database.GetPendingMigrations().Should().NotBeEmpty();
      }

      CreatePersistenceInitializer().Initialize();

      using (var context = ContextFactory.Create())
      {
        context.Database.GetPendingMigrations().Should().BeEmpty();
      }
    }

    [Fact]
    public void SeedsTheZipCodes()
    {
      CreatePersistenceInitializer().Initialize();

      using (var context = ContextFactory.Create())
      {
        var zipCodesCount = context.Zips.Count();
        zipCodesCount.Should().Be(13);

        var uniqueZipCodesCount = context.Zips
          .Select(_ => _.ZipCode)
          .Distinct()
          .Count();
        uniqueZipCodesCount.Should().Be(zipCodesCount);
      }
    }
  }
}