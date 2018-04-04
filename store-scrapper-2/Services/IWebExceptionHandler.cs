using System.Net;

namespace store_scrapper_2.Services
{
  public interface IWebExceptionHandler
  {
    bool ShouldBubbleUpException(WebException ex);
  }
}