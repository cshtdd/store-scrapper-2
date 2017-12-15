using System;
using FluentAssertions;
using store_scrapper_2.Model;
using store_scrapper_2;
using store_scrapper_2_int_Tests.Utils;
using Xunit;

namespace store_scrapper_2_int_Tests.DAL
{
  public class ZipCodeDataServiceTest : DatabaseTest
  {
    private readonly ZipCodeDataService _dataService;

    public ZipCodeDataServiceTest()
    {
      _dataService = new ZipCodeDataService(ContextFactory);
    }

    [Fact]
    public async void FindsTheDataForTheZipcode()
    {
      await CreatePersistenceInitializer().InitializeAsync();
      
      (await _dataService.ReadAsync("601"))
        .Should()
        .Be(new ZipCode("601", 18.16m, -66.72m));
      
      (await _dataService.ReadAsync("605"))
        .Should()
        .Be(new ZipCode("605", 18.43m, -67.15m));
      
      InvalidOperationException thrownException = null;
      try
      {
        await _dataService.ReadAsync("00000");
      }
      catch (InvalidOperationException ex)
      {
        thrownException = ex;
      }
      thrownException.Should().NotBeNull();
    }
  }
}