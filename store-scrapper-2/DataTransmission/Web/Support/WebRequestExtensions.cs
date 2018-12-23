using System;
using System.Diagnostics;
using System.Net;
using store_scrapper_2.Logging;

namespace store_scrapper_2.DataTransmission.Web.Support
{
  public static class WebRequestExtensions
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static string GetProxyString(this WebRequest request)
    {
      var proxy = request.Proxy as WebProxy;

      if (proxy == null)
      {
        return string.Empty;
      }

      return proxy.Address.AbsoluteUri;
    }
    
    public static string InstrumentedDownload(this WebRequest request, Func<WebRequest, String> downloader)
    {
      var stopwatch = Stopwatch.StartNew();
      
      var url = request.RequestUri.AbsoluteUri;
      Logger.LogDebug("Download", nameof(url), url, "Proxy", request.GetProxyString());

      try
      {
        var result = downloader(request);

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
  }
}