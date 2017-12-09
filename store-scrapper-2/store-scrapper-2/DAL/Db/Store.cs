using System;

namespace store_scrapper_2.DAL.Db
{
  public class Store
  {
    public int StoreId { get; set; }
    
    public string StoreNumber { get; set; }
    public string SatelliteNumber { get; set; }
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
    
    public DateTime? UpdateTimeUtc { get; set; }
  }
}