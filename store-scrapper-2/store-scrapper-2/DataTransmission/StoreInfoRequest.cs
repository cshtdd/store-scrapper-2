using System.Net;
using store_scrapper_2.Serialization;

namespace store_scrapper_2.DataTransmission
{
  public struct StoreInfoRequest
  {
    public string StoreNumber { get; }

    public StoreInfoRequest(string storeNumber) => StoreNumber = storeNumber;

    public override string ToString()
    {
      var q = new StoreLocatorQuery($"#{StoreNumber}", 4, "17", "SUBWAY_PROD");
      var qJson = q.ToJson();
      var qUrl = WebUtility.UrlEncode(qJson);

      var endpoint = "https://locator-svc.subway.com/v2//GetLocations.ashx";

      return $"{endpoint}?&q={qUrl}";
    }
  }
}