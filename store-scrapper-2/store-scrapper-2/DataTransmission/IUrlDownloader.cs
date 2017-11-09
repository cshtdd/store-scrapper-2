using System.Threading.Tasks;

namespace store_scrapper_2.DataTransmission
{
  public interface IUrlDownloader
  {
    Task<string> Download(string url);
  }
}