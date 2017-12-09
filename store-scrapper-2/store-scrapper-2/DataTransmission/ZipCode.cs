using System;
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
      if ((zip ?? string.Empty).Length != 5)
      {
        throw new ArgumentException($"Invalid {nameof(zip)} length");
      }

      if (!zip.ToCharArray().All(char.IsDigit))
      {
        throw new ArgumentException($"NonNumeric {nameof(zip)} found");        
      }
      
      Zip = zip;
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