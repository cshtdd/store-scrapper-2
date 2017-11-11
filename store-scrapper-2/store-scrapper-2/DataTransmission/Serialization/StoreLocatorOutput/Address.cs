using System.Runtime.Serialization;

namespace store
{
  [DataContract]
  public class Address
  {
    [DataMember(Order = 0)]
    public string Address1 { get; set; }
    [DataMember(Order = 1)]
    public string Address2 { get; set; }
    [DataMember(Order = 2)]
    public string Address3 { get; set; }
    [DataMember(Order = 3)]
    public string City { get; set; }
    [DataMember(Order = 4)]
    public string StateProvCode { get; set; }
    [DataMember(Order = 5)]
    public string PostalCode { get; set; }
    [DataMember(Order = 6)]
    public string CountryCode { get; set; }
    [DataMember(Order = 7)]
    public string CountryCode3 { get; set; }
  }
}