namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public interface IProxiedUrlDownloader
  {
    string Download(string url, ProxyInfo proxy);
  }
}