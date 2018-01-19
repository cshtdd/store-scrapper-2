﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2;
using store_scrapper_2.DataTransmission;
using store_scrapper_2_int_Tests.Utils;
using Xunit;

namespace store_scrapper_2_int_Tests.DAL
{
  public class StoreInfoResponseDataServiceTest : DatabaseTest
  {
    private readonly StoreInfoResponseDataService dataService;

    public StoreInfoResponseDataServiceTest() => dataService = new StoreInfoResponseDataService(ContextFactory);

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
      
      await Task.WhenAll(responses.Select(dataService.CreateNewAsync));
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(responses.Count);

        await Task.WhenAll(responses.Select(context.ShouldContainStoreEquivalentToAsync));
      }

      Task.WhenAll(responses.Select(_ => dataService.ContainsStoreAsync(_.StoreNumber)))
        .Result
        .All(_ => _)
        .Should()
        .BeTrue();

      (await dataService.ContainsStoreAsync("7777-3")).Should().BeFalse();
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
      
      await dataService.CreateNewAsync(responses);
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(responses.Count);

        await Task.WhenAll(responses.Select(context.ShouldContainStoreEquivalentToAsync));
      }

      Task.WhenAll(responses.Select(_ => dataService.ContainsStoreAsync(_.StoreNumber)))
        .Result
        .All(_ => _)
        .Should()
        .BeTrue();

      (await dataService.ContainsStoreAsync("7777-3")).Should().BeFalse();
    }
    
    [Fact]
    public async Task UpdatesAnExistingResponse()
    {
      await CreatePersistenceInitializer().InitializeAsync();

      var originalResponse = StoreInfoResponseFactory.Create("11111-3");

      await dataService.CreateNewAsync(originalResponse);
      (await dataService.ContainsStoreAsync(originalResponse.StoreNumber)).Should().BeTrue();
      
      var updatedResponse = CreateUpdatedResponse(originalResponse);

      await dataService.UpdateAsync(updatedResponse);
      (await dataService.ContainsStoreAsync(originalResponse.StoreNumber)).Should().BeTrue();
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(1);

        await context.ShouldContainStoreEquivalentToAsync(updatedResponse);
      }
    }

    [Fact]
    public async Task CannotInsertTheSameItemTwice()
    {
      await CreatePersistenceInitializer().InitializeAsync();
   
      var response = StoreInfoResponseFactory.Create("11111-3");

      await dataService.CreateNewAsync(response);

      ((Func<Task>) (async () =>
      {
        await dataService.CreateNewAsync(response);
      })).Should().Throw<DbUpdateException>();
    }

    [Fact]
    public async Task CannotUpdateANonExistingStore()
    {
      await CreatePersistenceInitializer().InitializeAsync();

      var response = StoreInfoResponseFactory.Create("11111-3");
      
      ((Func<Task>) (async () =>
      {
        await dataService.UpdateAsync(response);
      })).Should().Throw<InvalidOperationException>();
    }

    private static StoreInfo CreateUpdatedResponse(StoreInfo original)
    {
      var updatedResponse = StoreInfoResponseFactory.Create("00000-0");
      updatedResponse.StoreNumber = original.StoreNumber;
      return updatedResponse;
    }
  }
}