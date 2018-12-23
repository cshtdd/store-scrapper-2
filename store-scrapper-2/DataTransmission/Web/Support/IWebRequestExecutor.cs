using System.Net;

namespace store_scrapper_2.DataTransmission.Web.Support
{
  public interface IWebRequestExecutor
  {
    string Run(WebRequest request);
  }
}