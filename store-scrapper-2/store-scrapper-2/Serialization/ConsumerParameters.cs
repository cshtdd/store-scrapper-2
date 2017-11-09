using System.Runtime.Serialization;

namespace store_scrapper_2.Serialization
{
  [DataContract]
  public class ConsumerParameters
  {
    [DataMember(Order = 0, Name = "metric")]
    public bool Metric { get; set; }
    [DataMember(Order = 1, Name = "culture")]
    public string Culture { get; set; }
    [DataMember(Order = 2, Name = "country")]
    public string Country { get; set; }
    [DataMember(Order = 3, Name = "size")]
    public string Size { get; set; }
    [DataMember(Order = 4, Name = "template")]
    public string Template { get; set; }
    [DataMember(Order = 5, Name = "rtl")]
    public bool Rtl { get; set; }
    [DataMember(Order = 6, Name = "clientId")]
    public string ClientId { get; set; }
    [DataMember(Order = 7, Name = "key")]
    public string Key { get; set; }
  }
}