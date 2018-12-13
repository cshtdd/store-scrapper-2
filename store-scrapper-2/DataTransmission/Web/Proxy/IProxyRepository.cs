namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public interface IProxyRepository
  {
    ProxyInfo Read();
    void CountSuccessRequest(ProxyInfo proxy);
    void CountFailedRequest(ProxyInfo proxy);
  }
}