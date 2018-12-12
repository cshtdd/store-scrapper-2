using System;
using System.IO;
using System.Net;
using store_scrapper_2.Logging;

namespace store_scrapper_2.DataTransmission.WebRequests
{
  public class WebRequestExecutor : IWebRequestExecutor
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public string Run(HttpWebRequest request)
    {
      var url = request.RequestUri.ToString();
      
      try
      {
        Logger.LogDebug("Download", nameof(url), url);

        using (var response = request.GetResponse())
        using (var responseStream = response.GetResponseStream())
        using (var reader = new StreamReader(responseStream))
        {
          var result = reader.ReadToEnd();
          var kbytes = Convert.ToInt32(result.Length / 1024);

          Logger.LogInfo("Download", "KBytes", kbytes, "Result", true);
          
          return result;
        }
      }
      catch (WebException ex)
      {
        Logger.LogError("Download Error", ex, nameof(url), url);
        throw;
      }
    }
  }
}