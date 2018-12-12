namespace store_scrapper_2.DataTransmission.Proxy
{
  public interface IProxiedUrlDownloader
  {
    string Download(string url, ProxyInfo proxy);
  }
}