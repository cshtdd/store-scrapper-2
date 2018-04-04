using System.Net;

namespace store_scrapper_2.Services
{
  public class NeverIgnoreExceptions : IWebExceptionHandler
  {
    public bool ShouldBubbleUpException(WebException ex) => true;
  }
}