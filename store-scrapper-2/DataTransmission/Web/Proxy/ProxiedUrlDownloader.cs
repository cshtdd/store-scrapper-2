using System.IO;
using System.Net;
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
      try
      {
        return RunInternal(request);        
      }
      catch (IOException ex)
      {
        throw new WebException(ex.Message, ex);        
      }
    }

    private string RunInternal(HttpWebRequest request)
    {
      return _webRequestExecutor.Run(request);
    }
  }
}