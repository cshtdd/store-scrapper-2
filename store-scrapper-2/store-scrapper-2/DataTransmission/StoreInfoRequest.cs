using System.Net;
using store_scrapper_2.DataTransmission.Serialization;

namespace store_scrapper_2.DataTransmission
{
  public struct StoreInfoRequest
  {
    public string StoreNumber { get; }
    public string SatelliteNumber { get; }

    public string FullStoreNumber => $"{StoreNumber}-{SatelliteNumber}";

    public StoreInfoRequest(string storeNumber, string satelliteNumber)
    {
      StoreNumber = storeNumber;
      SatelliteNumber = satelliteNumber;
    }

    public string ToUrl()
    {
      var q = new StoreLocatorQuery($"#{FullStoreNumber}", 4, "17", "SUBWAY_PROD");
      var qJson = q.ToJson();
      var qUrl = WebUtility.UrlEncode(qJson);

      var endpoint = "https://locator-svc.subway.com/v2//GetLocations.ashx";

      return $"{endpoint}?&q={qUrl}";
    }
    
    public override string ToString() => $"StoreInfoResponse {FullStoreNumber}";
  }
}