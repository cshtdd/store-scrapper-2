using System.Net;
using store_scrapper_2.DataTransmission.Serialization;
using store_scrapper_2.Model;

namespace store_scrapper_2.DataTransmission
{
  public struct StoreInfoRequest
  {
    public StoreNumber StoreNumber { get; }

    public StoreInfoRequest(StoreNumber storeNumber) => StoreNumber = storeNumber;

    public string ToUrl()
    {
      var q = new StoreLocatorQuery($"#{StoreNumber}", 4, "17", "SUBWAY_PROD");
      var qJson = q.ToJson();
      var qUrl = WebUtility.UrlEncode(qJson);

      const string endpoint = "https://locator-svc.subway.com/v2//GetLocations.ashx";

      return $"{endpoint}?&q={qUrl}";
    }
    
    public override string ToString() => $"StoreInfoRequest {StoreNumber }";
  }
}