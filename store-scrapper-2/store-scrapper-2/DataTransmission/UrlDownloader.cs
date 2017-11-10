using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace store_scrapper_2.DataTransmission
{
  public class UrlDownloader : IUrlDownloader
  {
    public string Download(string url) => DownloadAsync(url).Result;

    public async Task<string> DownloadAsync(string url)
    {
      var request = WebRequest.CreateHttp(url);
      
      using (var response = request.GetResponse())
      using (var responseStream = response.GetResponseStream())
      using (var reader = new StreamReader(responseStream))
      {
        return await reader.ReadToEndAsync();
      }
    }
  }
}