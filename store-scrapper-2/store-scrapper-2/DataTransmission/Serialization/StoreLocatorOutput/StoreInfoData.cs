using System.Runtime.Serialization;

namespace store_scrapper_2
{
  [DataContract]
  public class StoreInfoData
  {
    [DataMember(Order = 0)]
    public LocationId LocationId { get; set; }
    [DataMember(Order = 1)]
    public Address Address { get; set; }
    [DataMember(Order = 2)]
    public Geo Geo { get; set; }
    [DataMember(Order = 3)]
    public int ListingNumber { get; set; }
    [DataMember(Order = 4)]
    public string OrderingUrl { get; set; }
    [DataMember(Order = 5)]
    public string CateringUrl { get; set; }
  }
}