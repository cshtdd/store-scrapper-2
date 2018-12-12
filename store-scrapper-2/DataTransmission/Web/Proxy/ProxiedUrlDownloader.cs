using store_scrapper_2.DataTransmission.Web.Support;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxiedUrlDownloader : IProxiedUrlDownloader
  {
    private readonly IWebRequestExecutor _webRequestExecutor;
    private readonly IWebRequestFactory _webRequestFactory;

    public ProxiedUrlDownloader(IWebRequestExecutor webRequestExecutor, IWebRequestFactory webRequestFactory)
    {
      _webRequestExecutor = webRequestExecutor;
      _webRequestFactory = webRequestFactory;
    }

    public string Download(string url, ProxyInfo proxy)
    {
      var request = _webRequestFactory.CreateHttp(url, proxy);
      return _webRequestExecutor.Run(request);
    }
  }
}