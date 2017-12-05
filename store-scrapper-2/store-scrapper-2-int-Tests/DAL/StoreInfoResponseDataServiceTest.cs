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
    private readonly StoreInfoResponseDataService dataService;

    public StoreInfoResponseDataServiceTest()
    {
      dataService = new StoreInfoResponseDataService(ContextFactory);
    }
    
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
      
      responses.ForEach(async _ => await dataService.CreateNewAsync(_));
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(responses.Count);

        responses.ForEach(async _ => await context.ShouldContainStoreEquivalentTo(_));
      }

      responses.ForEach(async _ => (await dataService.ContainsStoreAsync(_.StoreNumber, _.SatelliteNumber)).Should().BeTrue());

      (await dataService.ContainsStoreAsync("7777", "3")).Should().BeFalse();
    }

    [Fact]
    public async Task UpdatesAnExistingResponse()
    {
      await new PersistenceInitializer(ContextFactory).InitializeAsync();

      var originalResponse = StoreInfoResponseFactory.Create("11111-3");

      await dataService.CreateNewAsync(originalResponse);
      (await dataService.ContainsStoreAsync(originalResponse.StoreNumber, originalResponse.SatelliteNumber)).Should().BeTrue();
      
      var updatedResponse = CreateUpdatedResponse(originalResponse);

      await dataService.UpdateAsync(updatedResponse);
      (await dataService.ContainsStoreAsync(originalResponse.StoreNumber, originalResponse.SatelliteNumber)).Should().BeTrue();
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(1);

        await context.ShouldContainStoreEquivalentTo(updatedResponse);
      }
    }

    private static StoreInfoResponse CreateUpdatedResponse(StoreInfoResponse originalResponse)
    {
      var updatedResponse = StoreInfoResponseFactory.Create("00000-0");
      updatedResponse.StoreNumber = originalResponse.StoreNumber;
      updatedResponse.SatelliteNumber = originalResponse.SatelliteNumber;
      return updatedResponse;
    }
  }
}