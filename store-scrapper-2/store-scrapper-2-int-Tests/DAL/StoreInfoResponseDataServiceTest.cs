using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;
using store_scrapper_2_int_Tests.Utils;
using Xunit;

namespace store_scrapper_2_int_Tests.DAL
{
  public class StoreInfoResponseDataServiceTest : DatabaseTest
  {
    private readonly StoreInfoResponseDataService _dataService;

    public StoreInfoResponseDataServiceTest() => _dataService = new StoreInfoResponseDataService(ContextFactory);

    [Fact]
    [Obsolete]
    public async Task SavesANewResponse()
    {
      await CreatePersistenceInitializer().InitializeAsync();

      var responses = new List<StoreInfo>
      {
        StoreInfoResponseFactory.Create("11111-3"),
        StoreInfoResponseFactory.Create("22222-3"),
        StoreInfoResponseFactory.Create("33333-3")
      };

      using (var context = ContextFactory.Create())
      {
        context.Stores.Should().BeEmpty();
      }
      
      await Task.WhenAll(responses.Select(_dataService.CreateNewAsync));
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(responses.Count);

        await Task.WhenAll(responses.Select(context.ShouldContainStoreEquivalentToAsync));
      }

      Task.WhenAll(responses.Select(_ => _dataService.ContainsStoreAsync(_.StoreNumber)))
        .Result
        .All(_ => _)
        .Should()
        .BeTrue();

      (await _dataService.ContainsStoreAsync("7777-3")).Should().BeFalse();
    }

    [Fact]
    public async Task SavesNewResponses()
    {
      await CreatePersistenceInitializer().InitializeAsync();

      var responses = new List<StoreInfo>
      {
        StoreInfoResponseFactory.Create("11111-3"),
        StoreInfoResponseFactory.Create("22222-3"),
        StoreInfoResponseFactory.Create("33333-3")
      };

      using (var context = ContextFactory.Create())
      {
        context.Stores.Should().BeEmpty();
      }
      
      await _dataService.CreateNewAsync(responses);
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(responses.Count);

        await Task.WhenAll(responses.Select(context.ShouldContainStoreEquivalentToAsync));
      }

      Task.WhenAll(responses.Select(_ => _dataService.ContainsStoreAsync(_.StoreNumber)))
        .Result
        .All(_ => _)
        .Should()
        .BeTrue();

      (await _dataService.ContainsStoreAsync("7777-3")).Should().BeFalse();
    }
    
    [Fact]
    [Obsolete]
    public async Task UpdatesAnExistingResponse()
    {
      await CreatePersistenceInitializer().InitializeAsync();

      var originalResponse = StoreInfoResponseFactory.Create("11111-3");

      await _dataService.CreateNewAsync(originalResponse);
      (await _dataService.ContainsStoreAsync(originalResponse.StoreNumber)).Should().BeTrue();
      
      var updatedResponse = CreateUpdatedResponse(originalResponse);

      await _dataService.UpdateAsync(updatedResponse);
      (await _dataService.ContainsStoreAsync(originalResponse.StoreNumber)).Should().BeTrue();
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(1);

        await context.ShouldContainStoreEquivalentToAsync(updatedResponse);
      }
    }
    
    [Fact]
    public async Task UpdatesExistingResponses()
    {
      await CreatePersistenceInitializer().InitializeAsync();

      var seededResponses = StoreNumberFactory
        .Create(50)
        .Select(StoreInfoResponseFactory.Create)
        .ToArray();
      await _dataService.CreateNewAsync(seededResponses);
      (await ContainsAllStores(seededResponses)).Should().BeTrue();

      var originalResponses = seededResponses.Take(5);
      
      
      var updatedResponses = originalResponses
        .Select(CreateUpdatedResponse)
        .ToArray();     
      await _dataService.UpdateAsync(updatedResponses);
      (await ContainsAllStores(updatedResponses)).Should().BeTrue();

      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(50);
        await Task.WhenAll(updatedResponses.Select(context.ShouldContainStoreEquivalentToAsync));
      }
    }

    [Fact]
    public async Task CannotInsertTheSameItemTwice()
    {
      await CreatePersistenceInitializer().InitializeAsync();
   
      var response1 = StoreInfoResponseFactory.Create("11111-3");
      var response2 = StoreInfoResponseFactory.Create("11111-3");

      await _dataService.CreateNewAsync(response1);

      ((Func<Task>) (async () =>
      {
        await _dataService.CreateNewAsync(response1);
      })).Should().Throw<DbUpdateException>();

      ((Func<Task>) (async () =>
      {
        await _dataService.CreateNewAsync(response2);
      })).Should().Throw<DbUpdateException>();

      ((Func<Task>) (async () =>
      {
        await _dataService.CreateNewAsync(new [] { response1 });
      })).Should().Throw<DbUpdateException>();

      ((Func<Task>) (async () =>
      {
        await _dataService.CreateNewAsync(new [] { response2 });
      })).Should().Throw<DbUpdateException>();

      ((Func<Task>) (async () =>
      {
        await _dataService.CreateNewAsync(new [] { response1, response2 });
      })).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task CannotUpdateANonExistingStore()
    {
      await CreatePersistenceInitializer().InitializeAsync();

      await _dataService.CreateNewAsync(new []
      {
        StoreInfoResponseFactory.Create("11111-3"),
        StoreInfoResponseFactory.Create("22222-3"),
        StoreInfoResponseFactory.Create("33333-3")
      });

      var response = StoreInfoResponseFactory.Create("55555-3");
      
      ((Func<Task>) (async () =>
      {
        await _dataService.UpdateAsync(response);
      })).Should().Throw<InvalidOperationException>();
      
      ((Func<Task>) (async () =>
      {
        await _dataService.UpdateAsync(new [] { response });
      })).Should().Throw<InvalidOperationException>();
      
      ((Func<Task>) (async () =>
      {
        await _dataService.UpdateAsync(new [] { response, StoreInfoResponseFactory.Create("22222-3") });
      })).Should().Throw<InvalidOperationException>();
    }
    
    private static StoreInfo CreateUpdatedResponse(StoreInfo original)
    {
      var updatedResponse = StoreInfoResponseFactory.Create(original.StoreNumber);
      updatedResponse.Address1 = Guid.NewGuid().ToString();
      updatedResponse.Address2 = Guid.NewGuid().ToString();
      return updatedResponse;
    }

    private async Task<bool> ContainsAllStores(IEnumerable<StoreInfo> stores) => 
      await ContainsAllStoreNumbers(stores.Select(_ => _.StoreNumber));

    private async Task<bool> ContainsAllStoreNumbers(IEnumerable<StoreNumber> storeNumbers) => 
      (await Task.WhenAll(storeNumbers.Select(_dataService.ContainsStoreAsync))).All(_ => _);
  }
}