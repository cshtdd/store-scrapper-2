using System.Collections.Generic;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyListReader : IProxyListReader
  {
    private readonly IUrlDownloader _urlDownloader;

    public ProxyListReader(IUrlDownloader urlDownloader)
    {
      _urlDownloader = urlDownloader;
    }

    public IEnumerable<ProxyInfo> Read()
    {
      _urlDownloader.Download("https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list.txt");
      return null;
    }
  }
}