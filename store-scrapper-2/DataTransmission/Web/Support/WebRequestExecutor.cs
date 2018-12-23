using System.IO;
using System.Net;

namespace store_scrapper_2.DataTransmission.Web.Support
{
  public class WebRequestExecutor : IWebRequestExecutor
  {
    public string Run(WebRequest request)
    {
      using (var response = request.GetResponse())
      using (var responseStream = response.GetResponseStream())
      using (var reader = new StreamReader(responseStream))
      {
        return reader.ReadToEnd();
      }
    }
  }
}