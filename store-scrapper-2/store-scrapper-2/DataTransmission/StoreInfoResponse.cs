using System.Linq;
using System.Xml.Serialization;
using store_scrapper_2.DataTransmission.Serialization;

namespace store_scrapper_2.DataTransmission
{
  public struct StoreInfoResponse
  {
    public string StoreNumber { get; set; }
    public string SatelliteNumber { get; set; }
    
    public string FullStoreNumber => $"{StoreNumber}-{SatelliteNumber}";

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

    public int ListingNumber { get; set; }
    public string OrderingUrl { get; set; }
    public string CateringUrl { get; set; }

    public static StoreInfoResponse Parse(string responseText)
    {
      var json = responseText
        .TrimStart('(')
        .TrimEnd(')');
      
      var response = GenericJsonSerializer.FromJson<StoresLocatorResponse>(json);
      var storeData = response.ResultData.First();
      
      return new StoreInfoResponse
      {
        StoreNumber = storeData.LocationId.StoreNumber.ToString(),
        SatelliteNumber = storeData.LocationId.SatelliteNumber.ToString(),
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
        
        ListingNumber = storeData.ListingNumber,
        OrderingUrl = storeData.OrderingUrl,
        CateringUrl = storeData.CateringUrl
      };
    }

    public override string ToString() => $"StoreInfoResponse {FullStoreNumber}";
  }
}

