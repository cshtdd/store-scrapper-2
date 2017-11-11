using System.Runtime.Serialization;

namespace store
{
  [DataContract]
  public class Geo
  {
    [DataMember(Order = 0)]
    public double Latitude { get; set; }
    [DataMember(Order = 1)]
    public double Longitude { get; set; }
    [DataMember(Order = 2)]
    public string TimeZoneId { get; set; }
    [DataMember(Order = 3)]
    public int CurrentUtcOffset { get; set; }
  }
}