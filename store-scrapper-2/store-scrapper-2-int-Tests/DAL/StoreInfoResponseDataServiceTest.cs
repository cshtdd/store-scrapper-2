using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using store;
using store_scrapper_2;
using store_scrapper_2_int_Tests.Utils;
using Xunit;

namespace store_scrapper_2_int_Tests.DAL
{
  public class StoreInfoResponseDataServiceTest : DatabaseTest
  {   
    [Fact]
    public async Task SavesANewResponse()
    {
      await new PersistenceInitializer(ContextFactory).InitializeAsync();
      
      var response1 = StoreInfoResponseFactory.Create("11111-3");

      using (var context = ContextFactory.Create())
      {
        context.Stores.Should().BeEmpty();
      }

      var dataService = new StoreInfoResponseDataService(ContextFactory);
      await dataService.CreateNewAsync(response1);
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Should().NotBeEmpty();

        var dbStore = await context.Stores.FirstAsync(_ => _.StoreNumber == "11111" && _.SatelliteNumber == "3");
        dbStore.ShouldBeEquivalentTo(response1);
      }

      var insertedStoreExists = await dataService.ContainsStoreAsync("11111", "3");
      insertedStoreExists.Should().BeTrue();
      
      var differentStoreExists = await dataService.ContainsStoreAsync("22222", "3");
      differentStoreExists.Should().BeFalse();
    }
  }
}