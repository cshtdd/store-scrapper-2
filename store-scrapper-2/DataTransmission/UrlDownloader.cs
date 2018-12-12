using System.Net;
using store_scrapper_2.DataTransmission.WebRequests;

namespace store_scrapper_2.DataTransmission
{
  public class UrlDownloader : IUrlDownloader
  {
    private readonly IWebRequestExecutor _webRequestExecutor;

    public UrlDownloader(IWebRequestExecutor webRequestExecutor)
    {
      _webRequestExecutor = webRequestExecutor;
    }

    public string Download(string url)
    {
      var request = WebRequest.CreateHttp(url);
      return _webRequestExecutor.Run(request);
    }
  }
}
