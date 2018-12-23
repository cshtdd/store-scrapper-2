using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using store_scrapper_2.DataTransmission.Web.Support;
using store_scrapper_2.Logging;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxiedUrlDownloader : IProxiedUrlDownloader
  {
    private readonly IWebRequestExecutor _webRequestExecutor;
    private readonly IWebRequestFactory _webRequestFactory;

    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public ProxiedUrlDownloader(IWebRequestExecutor webRequestExecutor, IWebRequestFactory webRequestFactory)
    {
      _webRequestExecutor = webRequestExecutor;
      _webRequestFactory = webRequestFactory;
    }

    public string Download(string url, ProxyInfo proxy)
    {
      var stopwatch = Stopwatch.StartNew();
      
      var request = _webRequestFactory.CreateHttp(url, proxy);
      Logger.LogDebug("Download", nameof(url), url, "Proxy", request.GetProxyString());

      try
      {
        var result = DownloadInternal(request);

        var kbytes = Convert.ToInt32(result.Length / 1024);
        stopwatch.Stop();
        Logger.LogInfo("Download", "KBytes", kbytes, "Result", true, "ElapsedMs", stopwatch.ElapsedMilliseconds);

        return result;
      }
      catch (WebException e)
      {
        stopwatch.Stop();
        Logger.LogError("Download Error", e, nameof(url), url, "ElapsedMs", stopwatch.ElapsedMilliseconds);

        throw;
      }
    }

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