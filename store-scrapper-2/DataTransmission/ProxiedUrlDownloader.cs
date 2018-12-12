using System.Net;

namespace store_scrapper_2.DataTransmission
{
  public class ProxiedUrlDownloader : IProxiedUrlDownloader
  {
    private readonly IWebRequestExecutor _webRequestExecutor;

    public ProxiedUrlDownloader(IWebRequestExecutor webRequestExecutor)
    {
      _webRequestExecutor = webRequestExecutor;
    }

    public string Download(string url, ProxyInfo proxy)
    {
      var request = WebRequest.CreateHttp(url);
      request.Proxy = new WebProxy(proxy.IpAddress, proxy.Port);

      return _webRequestExecutor.Run(request);
    }
  }
}