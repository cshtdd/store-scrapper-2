using System.Runtime.Serialization;

namespace store_scrapper_2.Serialization
{
  [DataContract]
  public class DetectedLocation
  {
    [DataMember(Order = 0)]
    public int Latitude { get; set; }
    [DataMember(Order = 1)]
    public int Longitude { get; set; }
    [DataMember(Order = 2)]
    public int Accuracy { get; set; }
  }
}