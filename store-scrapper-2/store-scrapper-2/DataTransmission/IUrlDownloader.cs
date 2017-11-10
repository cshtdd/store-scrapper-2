using System.Threading.Tasks;

namespace store_scrapper_2.DataTransmission
{
  public interface IUrlDownloader
  {
    string Download(string url);
    Task<string> DownloadAsync(string url);
  }
}