using System.Net;

namespace store_scrapper_2.DataTransmission.WebRequests
{
  public interface IWebRequestExecutor
  {
    string Run(HttpWebRequest request);
  }
}