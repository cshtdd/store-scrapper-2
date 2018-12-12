namespace store_scrapper_2.DataTransmission
{
  public interface IProxiedUrlDownloader
  {
    string Download(string url, ProxyInfo proxy);
  }
}