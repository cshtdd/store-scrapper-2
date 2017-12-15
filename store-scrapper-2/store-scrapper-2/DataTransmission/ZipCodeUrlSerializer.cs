using System.Net;
using System.Runtime.Serialization.Json;
using store_scrapper_2.Model;
using store_scrapper_2.DataTransmission.Serialization;

namespace store_scrapper_2.DataTransmission
{
  public class ZipCodeUrlSerializer : IZipCodeUrlSerializer
  {
    public string ToUrl(ZipCode zipCode)
    {
      var q = new StoreLocatorQuery(zipCode.Zip, "17", "SUBWAY_PROD", zipCode.Latitude, zipCode.Longitude);
      var qJson = q.ToJson(new DataContractJsonSerializerSettings());
      var qUrl = WebUtility.UrlEncode(qJson);

      const string endpoint = "https://locator-svc.subway.com/v2//GetLocations.ashx";

      return $"{endpoint}?q={qUrl}";
    }
  }
}