namespace store_scrapper_2.DataTransmission
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

    public string Download(string url)
    {
      var request = _webRequestFactory.CreateHttp(url);
      return _webRequestExecutor.Run(request);
    }
  }
}
