namespace store_scrapper_2.DataTransmission
{
  public struct StoreInfoResponse
  {
    public string StoreNumber { get; }
    public string SatelliteNumber { get; }
    
    public string FullStoreNumber => $"{StoreNumber}-{SatelliteNumber}";

    public bool IsRestricted { get; }

    public string Address1 { get; }
    public string Address2 { get; }
    public string Address3 { get; }
    public string City { get; }
    public string State { get; }
    public string PostalCode { get; }
    public string CountryCode { get; }
    public string CountryCode3 { get; }
   
    public double Latitude { get; }
    public double Longitude { get; }
    public string TimeZoneId { get; }
    public int CurrentUtcOffset { get; }

    public int ListingNumber { get; }
    public string OrderingUrl { get; }
    public string CateringUrl { get; }
  }
}

