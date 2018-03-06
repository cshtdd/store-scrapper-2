using System.Runtime.Serialization;

namespace store_scrapper_2
{
  [DataContract]
  public class LocationId
  {
    [DataMember(Order = 0)]
    public int StoreNumber { get; set; }
    [DataMember(Order = 1)]
    public int SatelliteNumber { get; set; }
    [DataMember(Order = 2)]
    public bool IsRestricted { get; set; }
  }
}