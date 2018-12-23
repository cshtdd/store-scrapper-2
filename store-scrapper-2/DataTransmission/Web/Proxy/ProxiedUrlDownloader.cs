using System;
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

    public string Download(string url, ProxyInfo proxy) => 
      _webRequestFactory
        .CreateHttp(url, proxy)
        .InstrumentedDownload(DownloadInternal);

    private string DownloadInternal(WebRequest request)
    {
      try
      {
        return _webRequestExecutor.Run(request);
      }
      catch (OperationCanceledException e)
      {
        throw new WebException(e.Message, e);
      }
      catch (IOException e)
      {
        throw new WebException(e.Message, e);
      }
    }
  }
}