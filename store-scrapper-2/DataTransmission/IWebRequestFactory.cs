using System.Net;

namespace store_scrapper_2.DataTransmission
{
  public interface IWebRequestFactory
  {
    HttpWebRequest CreateHttp(string url);
    HttpWebRequest CreateHttp(string url, ProxyInfo proxy);
  }
}