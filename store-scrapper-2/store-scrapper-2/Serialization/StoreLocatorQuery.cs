using System.Runtime.Serialization;
using store;

namespace store_scrapper_2.Serialization
{
  [DataContract]
  public class StoreLocatorQuery
  {
    [DataMember(Order = 0)]
    public string InputText { get; set; }
    [DataMember(Order = 1)]
    public GeoCode GeoCode { get; set; } = new GeoCode();
    [DataMember(Order = 2)]
    public DetectedLocation DetectedLocation { get; set; } = new DetectedLocation();
    [DataMember(Order = 3)]
    public Paging Paging { get; set; } = new Paging {StartIndex = 1, PageSize = 10};
    [DataMember(Order = 4)]
    public ConsumerParameters ConsumerParameters { get; set; }
    [DataMember(Order = 4)]
    public string[] Filters { get; set; } = { };
    [DataMember(Order = 5)]
    public int LocationType { get; set; }

    public StoreLocatorQuery(string inputText, int locationType, string clientId, string key)
    {
      InputText = inputText;
      ConsumerParameters = new ConsumerParameters
      {
        Culture = "en-US",
        Country = "US",
        Size = "D",
        Template = string.Empty,
        ClientId = clientId,
        Key = key
      };
      LocationType = locationType;
    }
  }
}