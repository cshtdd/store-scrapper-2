using System.Net;

namespace store_scrapper_2.DataTransmission
{
  public interface IWebRequestExecutor
  {
    string Run(HttpWebRequest request);
  }
}