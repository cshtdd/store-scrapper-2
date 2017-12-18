namespace store_scrapper_2.Configuration
{
  public interface IConfigurationReader
  {
    string ReadString(string key);
    int ReadInt(string key);
  }
}