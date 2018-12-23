using System.Net;

namespace store_scrapper_2.DataTransmission.Web.Support
{
  public interface IWebRequestFactory
  {
    WebRequest CreateHttp(string url);
    WebRequest CreateHttp(string url, ProxyInfo proxy);
  }
}