namespace store_scrapper_2.Configuration
{
  public interface IConfigurationReader
  {
    string Read(string key);
  }
}