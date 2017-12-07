﻿using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;

namespace store_scrapper_2_int_Tests.Utils
{
  public static class StoreInfoResponseFactory
  {
    public static StoreInfoResponse Create(string fullStoreNumber) => new StoreInfoResponse
    {
      StoreNumber = new StoreNumber(fullStoreNumber),
      Address1 = $"{fullStoreNumber}addr1",
      Address2 = $"{fullStoreNumber}addr2",
      Address3 = $"{fullStoreNumber}addr3",
      CateringUrl = $"{fullStoreNumber}the catering",
      City = $"{fullStoreNumber}the city",
      CountryCode = $"{fullStoreNumber}us",
      CountryCode3 = $"{fullStoreNumber}usa",
      CurrentUtcOffset = 5,
      IsRestricted = true,
      Latitude = 12,
      Longitude = 16,
      ListingNumber = 3,
      OrderingUrl = $"{fullStoreNumber}the ordering",
      PostalCode = $"{fullStoreNumber}12345",
      State = $"{fullStoreNumber}fl",
      TimeZoneId = $"{fullStoreNumber}EST"
    };
  }
}