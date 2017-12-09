using System.Runtime.Serialization;
using store_scrapper_2.DataTransmission.Serialization.QueryParameters;

namespace store_scrapper_2.DataTransmission.Serialization
{
  [DataContract]
  public class StoreLocatorQuery
  {
    [DataMember(Order = 0)]
    public string InputText { get; set; }
    [DataMember(Order = 1)]
    public GeoCode GeoCode { get; set; }
    [DataMember(Order = 2)]
    public DetectedLocation DetectedLocation { get; set; } = new DetectedLocation();
    [DataMember(Order = 3)]
    public Paging Paging { get; set; }
    [DataMember(Order = 4)]
    public ConsumerParameters ConsumerParameters { get; set; }
    [DataMember(Order = 4)]
    public string[] Filters { get; set; } = { };
    [DataMember(Order = 5)]
    public int LocationType { get; set; }

    public StoreLocatorQuery(string inputText, string postalCode, int locationType, string clientId, string key, int pageSize)
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
      Paging = new Paging
      {
        StartIndex = 1,
        PageSize = pageSize
      };
      GeoCode = new GeoCode
      {
        PostalCode = postalCode
      };
      LocationType = locationType;
    }
  }
}