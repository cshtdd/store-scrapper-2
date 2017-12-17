using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace store_scrapper_2.DataTransmission
{
  public class UrlDownloader : IUrlDownloader
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public string Download(string url) => DownloadAsync(url).Result;

    public async Task<string> DownloadAsync(string url)
    {
      var request = WebRequest.CreateHttp(url);

      try
      {
        Logger.Debug($"Download; {url}");

        using (var response = request.GetResponse())
        using (var responseStream = response.GetResponseStream())
        using (var reader = new StreamReader(responseStream))
        {
          return await reader.ReadToEndAsync();
        }
      }
      catch (WebException ex)
      {
        Logger.Debug("Download Error", ex);
        throw;
      }
    }
  }
}