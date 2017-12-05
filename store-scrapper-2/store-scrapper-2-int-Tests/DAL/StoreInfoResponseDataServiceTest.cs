using System;
using System.Linq.Expressions;
using System.Threading;
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
    [Fact]
    public async Task SavesANewResponse()
    {
      await new PersistenceInitializer(ContextFactory).InitializeAsync();
      
      var response = new StoreInfoResponse
      {
        StoreNumber = "11111",
        SatelliteNumber = "3",
        Address1 = "addr1",
        Address2 = "addr2",
        Address3 = "addr3",
        CateringUrl = "the catering",
        City = "the city",
        CountryCode = "us",
        CountryCode3 = "usa",
        CurrentUtcOffset = 5,
        IsRestricted = true,
        Latitude = 12,
        Longitude = 16,
        ListingNumber = 3,
        OrderingUrl = "the ordering",
        PostalCode = "12345",
        State = "fl",
        TimeZoneId = "EST"
      };

      using (var context = ContextFactory.Create())
      {
        context.Stores.Should().BeEmpty();
      }

      var dataService = new StoreInfoResponseDataService(ContextFactory);
      await dataService.CreateNewAsync(response);
      
      using (var context = ContextFactory.Create())
      {
        context.Stores.Should().NotBeEmpty();

        var dbStore = await context.Stores.FirstAsync(_ => _.StoreNumber == "11111" && _.SatelliteNumber == "3");

        dbStore.Address1.Should().BeEquivalentTo(response.Address1);
        dbStore.Address2.Should().BeEquivalentTo(response.Address2);
        dbStore.Address3.Should().BeEquivalentTo(response.Address3);
        dbStore.CateringUrl.Should().BeEquivalentTo(response.CateringUrl);
        dbStore.City.Should().BeEquivalentTo(response.CateringUrl);
        dbStore.CountryCode.Should().BeEquivalentTo(response.CountryCode);
        dbStore.CountryCode3.Should().BeEquivalentTo(response.CountryCode3);
        dbStore.CurrentUtcOffset.Should().Be(response.CurrentUtcOffset);
        dbStore.IsRestricted.Should().Be(response.IsRestricted);
        dbStore.Latitude.Should().Be(response.Latitude);
        dbStore.Longitude.Should().Be(response.Longitude);
        dbStore.ListingNumber.Should().Be(response.ListingNumber);
        dbStore.OrderingUrl.Should().Be(response.OrderingUrl);
        dbStore.PostalCode.Should().Be(response.PostalCode);
        dbStore.SatelliteNumber.Should().Be(response.SatelliteNumber);
        dbStore.State.Should().Be(response.State);
        dbStore.StoreNumber.Should().Be(response.StoreNumber);
        dbStore.TimeZoneId.Should().Be(response.TimeZoneId);
      }

      var insertedStoreExists = await dataService.ContainsStoreAsync("11111", "3");
      insertedStoreExists.Should().BeTrue();
      
      var differentStoreExists = await dataService.ContainsStoreAsync("22222", "3");
      differentStoreExists.Should().BeFalse();
    }
  }
}