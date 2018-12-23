using System.Net;

namespace store_scrapper_2.DataTransmission.Web.Support
{
  public static class WebRequestExtensions
  {
    public static string GetProxyString(this WebRequest request)
    {
      var proxy = request.Proxy as WebProxy;

      if (proxy == null)
      {
        return string.Empty;
      }

      return proxy.Address.AbsoluteUri;
    }
  }
}