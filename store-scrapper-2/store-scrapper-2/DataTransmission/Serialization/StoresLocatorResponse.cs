using System.Runtime.Serialization;
using store;

namespace store_scrapper_2.DataTransmission.Serialization
{
  [DataContract]
  public class StoresLocatorResponse
  {
    [DataMember(Order = 0)]
    public StoreInfoData[] ResultData { get; set; }
    [DataMember(Order = 1)]
    public string[] ResultHtml { get; set; }
    [DataMember(Order = 2)]
    public string PagingHtml { get; set; }
    [DataMember(Order = 3)]
    public string ResultsHeadingHtml { get; set; }
  }
}