using System.IO;
using System.Net;

using store_scrapper_2.Logging;

namespace store_scrapper_2.DataTransmission
{
  public class UrlDownloader : IUrlDownloader
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public string Download(string url)
    {
      var request = WebRequest.CreateHttp(url);

      try
      {
        Logger.LogDebug("Download", nameof(url), url);

        using (var response = request.GetResponse())
        using (var responseStream = response.GetResponseStream())
        using (var reader = new StreamReader(responseStream))
        {
          return reader.ReadToEnd();
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
