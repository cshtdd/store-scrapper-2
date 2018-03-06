using System.Runtime.Serialization;
using store_scrapper_2.DataTransmission.Serialization.QueryParameters;

namespace store_scrapper_2.DataTransmission.Serialization
{
  [DataContract]
  public class StoreLocatorQuery
  {
    [DataMember(Order = 0)]
    public string InputText { get; set; } = string.Empty;
    [DataMember(Order = 1)]
    public GeoCode GeoCode { get; set; }
    [DataMember(Order = 2)]
    public DetectedLocation DetectedLocation { get; set; } = new DetectedLocation();
    [DataMember(Order = 3)]
    public Paging Paging { get; set; } = new Paging {StartIndex = 1, PageSize = 50};
    [DataMember(Order = 4)]
    public ConsumerParameters ConsumerParameters { get; set; }
    [DataMember(Order = 4)]
    public string[] Filters { get; set; } = { };
    [DataMember(Order = 5)]
    public int LocationType { get; set; } = 1;

    public StoreLocatorQuery(string zipCode, string clientId, string key, decimal latitude, decimal longitude)
    {
      GeoCode = new GeoCode
      {
        PostalCode = zipCode,
        Latitude = latitude,
        Longitude = longitude
      };
      ConsumerParameters = new ConsumerParameters
      {
        Culture = "en-US",
        Country = "US",
        Size = "D",
        Template = string.Empty,
        ClientId = clientId,
        Key = key
      };
    }
  }
}