using System.Net;

namespace store_scrapper_2.DataTransmission.Web.Support
{
  public class WebRequestFactory : IWebRequestFactory
  {
    public HttpWebRequest CreateHttp(string url) => WebRequest.CreateHttp(url);

    public HttpWebRequest CreateHttp(string url, ProxyInfo proxy)
    {
      var request = WebRequest.CreateHttp(url);
      request.Proxy = new WebProxy(proxy.IpAddress, proxy.Port);
      return request;
    }
  }
}