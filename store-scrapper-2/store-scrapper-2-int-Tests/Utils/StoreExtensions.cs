using FluentAssertions;
using store_scrapper_2;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;

namespace store_scrapper_2_int_Tests.Utils
{
  public static class StoreExtensions
  {
    public static void ShouldBeEquivalentTo(this Store sender, StoreInfoResponse response)
    {
      sender.Address1.Should().BeEquivalentTo(response.Address1);
      sender.Address2.Should().BeEquivalentTo(response.Address2);
      sender.Address3.Should().BeEquivalentTo(response.Address3);
      sender.CateringUrl.Should().BeEquivalentTo(response.CateringUrl);
      sender.City.Should().BeEquivalentTo(response.City);
      sender.CountryCode.Should().BeEquivalentTo(response.CountryCode);
      sender.CountryCode3.Should().BeEquivalentTo(response.CountryCode3);
      sender.CurrentUtcOffset.Should().Be(response.CurrentUtcOffset);
      sender.IsRestricted.Should().Be(response.IsRestricted);
      sender.Latitude.Should().Be(response.Latitude);
      sender.Longitude.Should().Be(response.Longitude);
      sender.ListingNumber.Should().Be(response.ListingNumber);
      sender.OrderingUrl.Should().Be(response.OrderingUrl);
      sender.PostalCode.Should().Be(response.PostalCode);
      sender.State.Should().Be(response.State);
      sender.TimeZoneId.Should().Be(response.TimeZoneId);
      new StoreNumber(sender.StoreNumber, sender.SatelliteNumber)
        .Should()
        .Be(response.StoreNumber);
    }
  }
}