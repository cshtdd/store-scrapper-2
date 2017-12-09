using System.IO;
using System.Runtime.Serialization.Json;

namespace store_scrapper_2.DataTransmission.Serialization
{
  public static class GenericJsonSerializer
  {
    public static string ToJson(this object sender, DataContractJsonSerializerSettings serializationSettings = null)
    {
      using (var stream = new MemoryStream())
      {
        var serializer = new DataContractJsonSerializer(sender.GetType(), serializationSettings);
        serializer.WriteObject(stream, sender);
        stream.Position = 0;

        using (var streamReader = new StreamReader(stream))
        {          
          return streamReader.ReadToEnd();
        }
      }
    }

    public static T FromJson<T>(string json, DataContractJsonSerializerSettings serializationSettings = null)
    {
      using (var stream = new MemoryStream())
      using (var streamWriter = new StreamWriter(stream))
      {
        streamWriter.Write(json);
        streamWriter.Flush();
        stream.Position = 0;
        
        var serializer = new DataContractJsonSerializer(typeof(T), serializationSettings);
        return (T) serializer.ReadObject(stream);
      }
    }
  }
}