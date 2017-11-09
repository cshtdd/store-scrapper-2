using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace store_scrapper_2.Serialization
{
  [DataContract]
  public class StoreLocatorQuery
  {
    [DataMember(Order = 0)]
    public string InputText { get; set; }
    [DataMember(Order = 1)]
    public string[] Filters { get; set; } = { };
    [DataMember(Order = 2)]
    public int LocationType { get; set; }


    public override string ToString()
    {
      using (var stream = new MemoryStream())
      {
        var ser = new DataContractJsonSerializer(typeof(StoreLocatorQuery));
        ser.WriteObject(stream, this);
        stream.Position = 0;

        using (var streamReader = new StreamReader(stream))
        {          
          return streamReader.ReadToEnd();
        }
      }
    }
  }
}