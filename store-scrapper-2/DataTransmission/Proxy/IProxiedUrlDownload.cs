namespace store_scrapper_2.DataTransmission.Proxy
{
  public interface IProxiedUrlDownload
  {
    string Download(string url, ProxyInfo proxy);
  }
}