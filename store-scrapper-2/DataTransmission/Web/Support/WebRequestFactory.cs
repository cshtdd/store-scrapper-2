using System.Net;
using store_scrapper_2.Configuration;

namespace store_scrapper_2.DataTransmission.Web.Support
{
  public class WebRequestFactory : IWebRequestFactory
  {
    private readonly IConfigurationReader _configurationReader;

    public WebRequestFactory(IConfigurationReader configurationReader)
    {
      _configurationReader = configurationReader;
    }
    
    public HttpWebRequest CreateHttp(string url) => WebRequest.CreateHttp(url);

    public HttpWebRequest CreateHttp(string url, ProxyInfo proxy)
    {
      var request = WebRequest.CreateHttp(url);
      request.Proxy = new WebProxy(proxy.IpAddress, proxy.Port);

      var timeoutMs = ReadProxyTimeout();
      request.Timeout = timeoutMs;
      request.ReadWriteTimeout = timeoutMs;
      
      return request;
    }

    private int ReadProxyTimeout() => _configurationReader.ReadInt(ConfigurationKeys.ProxyTimeoutMs, 30000);
  }
}