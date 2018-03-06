using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace store_scrapper_2.DataTransmission.Serialization.QueryParameters
{
  [DataContract]
  public class GeoCode
  {
    [DataMember(Order = 0)]
    public decimal Latitude { get; set; }
    [DataMember(Order = 1)]
    public decimal Longitude { get; set; }
    
    [DataMember(Order = 2)]
    public int? Accuracy { get; set; }
    [DataMember(Order = 3)]
    public string CountryCode { get; set; }
    [DataMember(Order = 4)]
    public string RegionCode { get; set; }
    [DataMember(Order = 5)]
    public string PostalCode { get; set; }
    [DataMember(Order = 6)]
    public string City { get; set; }
    [DataMember(Order = 7)]
    public string LocalityType { get; set; }
  }
}