using System.Runtime.Serialization;

namespace store_scrapper_2.DataTransmission.Serialization.QueryParameters
{
  [DataContract]
  public class Paging
  {
    [DataMember(Order = 0)]
    public int StartIndex { get; set; }
    [DataMember(Order = 1)]
    public int PageSize { get; set; }
  }
}