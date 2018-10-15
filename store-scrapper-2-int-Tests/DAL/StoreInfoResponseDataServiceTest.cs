using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;
using store_scrapper_2_int_Tests.Utils;
using store_scrapper_2_Tests.Factory;
using Xunit;

namespace store_scrapper_2_int_Tests.DAL
{
  public class StoreInfoResponseDataServiceTest : DatabaseTest
  {
    private readonly StoreInfoResponseDataService _dataService;

    public StoreInfoResponseDataServiceTest() => _dataService = new StoreInfoResponseDataService(ContextFactory);

    [Fact]
    public void SavesNewResponses()
    {
      CreatePersistenceInitializer().Initialize();

      var storeNumbers = StoreNumberFactory
        .Create(5)
        .ToArray();
      var responses = storeNumbers
        .Select(StoreInfoFactory.Create)
        .ToArray();

      StoresTableShouldBeEmpty();

      _dataService.CreateNew(new StoreInfo[] { });

      StoresTableShouldBeEmpty();

      _dataService.CreateNew(responses);
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(responses.Length);
        
        foreach (var r in responses)
        {
          context.ShouldContainStoreEquivalentTo(r);
        }
      }

      var foundStoreNumbers = (_dataService.ContainsStore(storeNumbers)).ToArray();
      foundStoreNumbers.Should().BeEquivalentTo(storeNumbers);
      
      var searchedNumbers = new List<StoreNumber>(storeNumbers);
      searchedNumbers.AddRange(new StoreNumber[] {"7777-3", "99999-0"});
      foundStoreNumbers = (_dataService.ContainsStore(searchedNumbers)).ToArray();
      foundStoreNumbers.Should().BeEquivalentTo(storeNumbers);
      
      _dataService.ContainsStore(new StoreNumber[] {"7777-3"}).Should().BeEmpty();

      _dataService.ContainsStore(new StoreNumber[] { }).Should().BeEmpty();
    }

    [Fact]
    public void UpdatesExistingResponses()
    {
      CreatePersistenceInitializer().Initialize();

      var seededResponses = StoreNumberFactory
        .Create(50)
        .Select(StoreInfoFactory.Create)
        .ToArray();
      _dataService.CreateNew(seededResponses);
      ContainsAllStores(seededResponses).Should().BeTrue();

      var originalResponses = seededResponses.Take(5);
      
      
      var updatedResponses = originalResponses
        .Select(CreateUpdatedResponse)
        .ToArray();     
      _dataService.Update(updatedResponses);
      ContainsAllStores(updatedResponses).Should().BeTrue();

      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Count().Should().Be(50);
        foreach (var r in updatedResponses)
        {
          context.ShouldContainStoreEquivalentTo(r);
        }
      }

      _dataService.Update(new StoreInfo[] { });
    }

    [Fact]
    public void CannotInsertTheSameItemTwice()
    {
      CreatePersistenceInitializer().Initialize();
   
      var response1 = StoreInfoFactory.Create("11111-3");
      var response2 = StoreInfoFactory.Create("11111-3");

      _dataService.CreateNew(new [] { response1 });
      
      ((Action) (() =>
      {
        _dataService.CreateNew(new [] { response1 });
      })).Should().Throw<DbUpdateException>();

      ((Action) (() =>
      {
        _dataService.CreateNew(new [] { response2 });
      })).Should().Throw<DbUpdateException>();

      ((Action) (() =>
      {
        _dataService.CreateNew(new [] { response1, response2 });
      })).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void CannotUpdateANonExistingStore()
    {
      CreatePersistenceInitializer().Initialize();

      _dataService.CreateNew(new []
      {
        StoreInfoFactory.Create("11111-3"),
        StoreInfoFactory.Create("22222-3"),
        StoreInfoFactory.Create("33333-3")
      });

      var response = StoreInfoFactory.Create("55555-3");
        
      ((Action) (() =>
      {
        _dataService.Update(new [] { response });
      })).Should().Throw<InvalidOperationException>();
      
      ((Action) (() =>
      {
        _dataService.Update(new [] { response, StoreInfoFactory.Create("22222-3") });
      })).Should().Throw<InvalidOperationException>();
    }
    
    private static StoreInfo CreateUpdatedResponse(StoreInfo original)
    {
      var updatedResponse = StoreInfoFactory.Create(original.StoreNumber);
      updatedResponse.Address1 = Guid.NewGuid().ToString();
      updatedResponse.Address2 = Guid.NewGuid().ToString();
      return updatedResponse;
    }

    private bool ContainsAllStores(IEnumerable<StoreInfo> stores) => 
      ContainsAllStoreNumbers(stores.Select(_ => _.StoreNumber));

    private bool ContainsAllStoreNumbers(IEnumerable<StoreNumber> storeNumbersEnumerable)
    {
      var storeNumbers = storeNumbersEnumerable.OrderBy(_ => _.ToString()).ToArray();
      
      var result = _dataService.ContainsStore(storeNumbers)
        .OrderBy(_ => _.ToString())
        .SequenceEqual(storeNumbers);

      return result;
    }
    
    private static void StoresTableShouldBeEmpty()
    {
      using (var context = ContextFactory.Create())
      {
        context.Stores.Should().BeEmpty();
      }
    }
  }
}