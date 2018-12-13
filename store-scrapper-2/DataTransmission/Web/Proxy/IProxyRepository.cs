namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public interface IProxyRepository
  {
    ProxyInfo Read();
    void MarkGoodRequest(ProxyInfo proxy);
    void MarkBadRequest(ProxyInfo proxy);
  }
}