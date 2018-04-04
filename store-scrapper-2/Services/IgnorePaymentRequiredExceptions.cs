using System.Net;

namespace store_scrapper_2.Services
{
  public class IgnorePaymentRequiredExceptions : IWebExceptionHandler
  {
    public bool ShouldBubbleUpException(WebException ex) => !ex.Message.Contains("402");
  }
}