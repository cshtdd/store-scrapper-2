using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using store_scrapper_2;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;
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
      
      foreach (var response in responses)
      {
        await dataService.CreateNewAsync(response);
      }
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(responses.Count);

        foreach (var response in responses)
        {
          await context.ShouldContainStoreEquivalentTo(response);
        }
      }

      foreach (var response in responses)
      {
        (await dataService.ContainsStoreAsync(response.StoreNumber)).Should().BeTrue();
      }

      (await dataService.ContainsStoreAsync(new StoreNumber("7777-3"))).Should().BeFalse();
    }

    [Fact]
    public async Task UpdatesAnExistingResponse()
    {
      await new PersistenceInitializer(ContextFactory).InitializeAsync();

      var originalResponse = StoreInfoResponseFactory.Create("11111-3");

      await dataService.CreateNewAsync(originalResponse);
      (await dataService.ContainsStoreAsync(originalResponse.StoreNumber)).Should().BeTrue();
      
      var updatedResponse = CreateUpdatedResponse(originalResponse);

      await dataService.UpdateAsync(updatedResponse);
      (await dataService.ContainsStoreAsync(originalResponse.StoreNumber)).Should().BeTrue();
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(1);

        await context.ShouldContainStoreEquivalentTo(updatedResponse);
      }
    }

    [Fact]
    public async Task CannotInsertTheSameItemTwice()
    {
      await new PersistenceInitializer(ContextFactory).InitializeAsync();
   
      var response = StoreInfoResponseFactory.Create("11111-3");

      await dataService.CreateNewAsync(response);

      InvalidOperationException thrownException = null;
      try
      {
        await dataService.CreateNewAsync(response);
      }
      catch (InvalidOperationException ex)
      {
        thrownException = ex;
      }
      thrownException.Should().NotBeNull();
    }

    [Fact]
    public async Task CannotUpdateANonExistingStore()
    {
      await new PersistenceInitializer(ContextFactory).InitializeAsync();

      var response = StoreInfoResponseFactory.Create("11111-3");

      InvalidOperationException thrownException = null;
      try
      {
        await dataService.UpdateAsync(response);
      }
      catch (InvalidOperationException ex)
      {
        thrownException = ex;
      }
      thrownException.Should().NotBeNull();
    }

    private static StoreInfoResponse CreateUpdatedResponse(StoreInfoResponse originalResponse)
    {
      var updatedResponse = StoreInfoResponseFactory.Create("00000-0");
      updatedResponse.StoreNumber = originalResponse.StoreNumber;
      return updatedResponse;
    }
  }
}