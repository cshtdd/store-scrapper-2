using System.Runtime.Serialization;

namespace store_scrapper_2.DataTransmission.Serialization.QueryParameters
{
  [DataContract]
  public class GeoCode
  {
    [DataMember(Order = 0)]
    public int Latitude { get; set; }    
    [DataMember(Order = 1)]
    public int Longitude { get; set; }
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