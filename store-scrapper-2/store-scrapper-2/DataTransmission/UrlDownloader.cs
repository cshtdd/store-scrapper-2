using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace store_scrapper_2.DataTransmission
{
  public class UrlDownloader : IUrlDownloader
  {
    public async Task<string> Download(string url)
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