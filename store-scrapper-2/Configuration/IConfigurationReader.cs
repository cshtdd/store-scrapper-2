namespace store_scrapper_2.Configuration
{
  public interface IConfigurationReader
  {
    string ReadString(string key);
    int ReadInt(string key, int defaultValue = 0);
    uint ReadUInt(string key, uint defaultValue = 0);
    bool ReadBool(string key, bool defaultValue = false);
  }
}