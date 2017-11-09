using System.IO;
using System.Runtime.Serialization.Json;

namespace store_scrapper_2.Serialization.Util
{
  public static class GenericJsonSerializer
  {
    public static string toJson(this object sender)
    {
      using (var stream = new MemoryStream())
      {
        var ser = new DataContractJsonSerializer(sender.GetType());
        ser.WriteObject(stream, sender);
        stream.Position = 0;

        using (var streamReader = new StreamReader(stream))
        {          
          return streamReader.ReadToEnd();
        }
      }
    }
  }
}