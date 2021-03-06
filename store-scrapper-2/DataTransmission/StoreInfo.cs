﻿using System;
using store_scrapper_2.Model;

namespace store_scrapper_2.DataTransmission
{
  public struct StoreInfo
  {
    public StoreNumber StoreNumber { get; set; }
    public bool IsRestricted { get; set; }

    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Address3 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string CountryCode { get; set; }
    public string CountryCode3 { get; set; }
   
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string TimeZoneId { get; set; }
    public int CurrentUtcOffset { get; set; }

    public string OrderingUrl { get; set; }
    public string CateringUrl { get; set; }

    public DateTime UpdateTimeUtc { get; set; }
    
    public static StoreInfo Create(StoreInfoData storeData) => new StoreInfo
    {
      StoreNumber = storeData.LocationId.ToStoreNumber(),
      IsRestricted = storeData.LocationId.IsRestricted,
        
      Address1 = storeData.Address.Address1,
      Address2 = storeData.Address.Address2,
      Address3 = storeData.Address.Address3,
      City = storeData.Address.City,
      State = storeData.Address.StateProvCode,
      PostalCode = storeData.Address.PostalCode,
      CountryCode = storeData.Address.CountryCode,
      CountryCode3 = storeData.Address.CountryCode3,
        
      Latitude = storeData.Geo.Latitude,
      Longitude = storeData.Geo.Longitude,
      TimeZoneId = storeData.Geo.TimeZoneId,
      CurrentUtcOffset = storeData.Geo.CurrentUtcOffset,

      OrderingUrl = storeData.OrderingUrl,
      CateringUrl = storeData.CateringUrl,
      
      UpdateTimeUtc = DateTime.UtcNow
    };

    public override string ToString() => $"{nameof(StoreInfo)} {StoreNumber}";
  }

  internal static class LocationIdExtensions
  {
    public static StoreNumber ToStoreNumber(this LocationId sender) => new StoreNumber(sender.StoreNumber, sender.SatelliteNumber);
  } 
}

