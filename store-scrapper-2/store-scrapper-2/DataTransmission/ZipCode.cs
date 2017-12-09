using System.Net;
using store_scrapper_2.DataTransmission.Serialization;

namespace store_scrapper_2.DataTransmission
{
  public struct ZipCode
  {
    public string Zip { get; }

    public ZipCode(string zip) => Zip = zip;

    public string ToUrl()
    {
      var q = new StoreLocatorQuery(string.Empty, $"{Zip}", 1, "17", "SUBWAY_PROD", 50);
      var qJson = q.ToJson();
      var qUrl = WebUtility.UrlEncode(qJson);

      const string endpoint = "https://locator-svc.subway.com/v2//GetLocations.ashx";

      return $"{endpoint}?q={qUrl}";
    }
    
    public override string ToString() => $"{nameof(ZipCode)} {Zip}";
  }
}