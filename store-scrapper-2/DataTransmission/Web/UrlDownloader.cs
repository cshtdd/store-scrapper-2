using System.Net;
using store_scrapper_2.DataTransmission.Web.Support;

namespace store_scrapper_2.DataTransmission.Web
{
  public class UrlDownloader : IUrlDownloader
  {
    private readonly IWebRequestExecutor _webRequestExecutor;
    private readonly IWebRequestFactory _webRequestFactory;
   
    public UrlDownloader(IWebRequestExecutor webRequestExecutor, IWebRequestFactory webRequestFactory)
    {
      _webRequestExecutor = webRequestExecutor;
      _webRequestFactory = webRequestFactory;
    }

    public string Download(string url) => 
      _webRequestFactory
        .CreateHttp(url)
        .InstrumentedDownload(DownloadInternal);

    private string DownloadInternal(WebRequest request) => _webRequestExecutor.Run(request);
  }
}
