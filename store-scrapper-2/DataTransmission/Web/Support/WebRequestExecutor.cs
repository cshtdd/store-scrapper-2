using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using store_scrapper_2.Logging;

namespace store_scrapper_2.DataTransmission.Web.Support
{
  public class WebRequestExecutor : IWebRequestExecutor
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public string Run(HttpWebRequest request)
    {
      var url = request.RequestUri.AbsoluteUri;

      var stopwatch = Stopwatch.StartNew();

      try
      {
        Logger.LogDebug("Download", nameof(url), url, "Proxy", GetProxyString(request));

        using (var response = request.GetResponse())
        using (var responseStream = response.GetResponseStream())
        using (var reader = new StreamReader(responseStream))
        {
          var result = reader.ReadToEnd();
          var kbytes = Convert.ToInt32(result.Length / 1024);

          stopwatch.Stop();
          Logger.LogInfo("Download", "KBytes", kbytes, "Result", true, "ElapsedMs", stopwatch.ElapsedMilliseconds);
          
          return result;
        }
      }
      catch (WebException ex)
      {
        stopwatch.Stop();
        Logger.LogError("Download Error", ex, nameof(url), url, "ElapsedMs", stopwatch.ElapsedMilliseconds);
        throw;
      }
    }

    private static string GetProxyString(WebRequest request)
    {
      var proxy = request.Proxy as WebProxy;

      if (proxy == null)
      {
        return string.Empty;
      }

      return proxy.Address.AbsoluteUri;
    }
  }
}