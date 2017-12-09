﻿using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using store_scrapper_2.DataTransmission.Serialization;

namespace store_scrapper_2.DataTransmission
{
  public struct ZipCode
  {
    public string Zip { get; }
    public decimal Latitude { get; }
    public decimal Longitude { get; }

    public ZipCode(string zip, decimal latitude, decimal longitude)
    {
      var zipCode = zip ?? string.Empty;
      
      if (zipCode.Length < 3 || zipCode.Length > 5)
      {
        throw new ArgumentException($"Invalid {nameof(zip)} length");
      }

      if (!zipCode.ToCharArray().All(char.IsDigit))
      {
        throw new ArgumentException($"NonNumeric {nameof(zip)} found");        
      }

      if (latitude < -90m || latitude > 90m)
      {
        throw new ArgumentException($"OutOfRange {nameof(latitude)} found");
      }

      if (longitude < -180m || longitude > 180m)
      {        
        throw new ArgumentException($"OutOfRange {nameof(longitude)} found");
      }
      
      Zip = zipCode;
      Latitude = latitude;
      Longitude = longitude;
    }

    public string ToUrl()
    {
      var q = new StoreLocatorQuery(Zip, "17", "SUBWAY_PROD", Latitude, Longitude);
      var qJson = q.ToJson(new DataContractJsonSerializerSettings());
      var qUrl = WebUtility.UrlEncode(qJson);

      const string endpoint = "https://locator-svc.subway.com/v2//GetLocations.ashx";

      return $"{endpoint}?q={qUrl}";
    }
    
    public override string ToString() => $"Zip={Zip}; Latitude={Latitude:F8}; Longitude={Longitude:F8};";
  }
}