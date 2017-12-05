using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using store_scrapper_2;
using store_scrapper_2.DataTransmission;
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

      var responses = new List<StoreInfoResponse>
      {
        StoreInfoResponseFactory.Create("11111-3"),
        StoreInfoResponseFactory.Create("22222-3"),
        StoreInfoResponseFactory.Create("33333-3")
      };

      using (var context = ContextFactory.Create())
      {
        context.Stores.Should().BeEmpty();
      }

      var dataService = new StoreInfoResponseDataService(ContextFactory);
      
      responses.ForEach(async _ => await dataService.CreateNewAsync(_));
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(responses.Count);

        responses.ForEach(async _ => await context.ShouldContainStoreEquivalentTo(_));
      }

      responses.ForEach(async _ => (await dataService.ContainsStoreAsync(_.StoreNumber, _.SatelliteNumber)).Should().BeTrue());

      (await dataService.ContainsStoreAsync("7777", "3")).Should().BeFalse();
    }
  }
}