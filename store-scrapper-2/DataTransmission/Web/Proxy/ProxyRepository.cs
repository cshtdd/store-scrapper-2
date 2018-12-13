using System;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyRepository : IProxyRepository
  {
    private readonly IProxyListReader _proxyListReader;

    public ProxyRepository(IProxyListReader proxyListReader)
    {
      _proxyListReader = proxyListReader;
    }

    public ProxyInfo Read()
    {
      throw new System.NotImplementedException();
    }

    public void MarkGoodRequest(ProxyInfo proxy)
    {
      throw new InvalidOperationException("No Proxy has been read");
    }

    public void MarkBadRequest(ProxyInfo proxy)
    {
      throw new InvalidOperationException("No Proxy has been read");
    }
  }
}