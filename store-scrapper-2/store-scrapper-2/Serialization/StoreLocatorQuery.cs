using System.Runtime.Serialization;
using store_scrapper_2.Serialization.Util;

namespace store_scrapper_2.Serialization
{
  [DataContract]
  public class StoreLocatorQuery
  {
    [DataMember(Order = 0)]
    public string InputText { get; set; }

    [DataMember(Order = 1)]
    public GeoCode GeoCode { get; set; }
    
    [DataMember(Order = 2)]
    public string[] Filters { get; set; } = { };
    [DataMember(Order = 3)]
    public int LocationType { get; set; }

    public StoreLocatorQuery(string inputText, int locationType)
    {
      InputText = inputText;
      LocationType = locationType;
      GeoCode = new GeoCode();
    }
    
    public override string ToString()
    {
      return this.toJson();
    }
  }
}