using Microsoft.Extensions.Configuration;

namespace store_scrapper_2.Configuration
{
  public class ConfigurationReader : IConfigurationReader
  {
    private IConfigurationRoot _configuration;
    public string EnvironmentName { get; }

    public ConfigurationReader() : this("PROD") { }
    public ConfigurationReader(string environmentName)
    {
      EnvironmentName = environmentName;
      BuildConfiguration();
    }

    private void BuildConfiguration()
    {
      var builder = new ConfigurationBuilder()
        .AddJsonFile("config.json", true)
        .AddJsonFile($"config.{EnvironmentName}.json", true)
        .AddEnvironmentVariables();
      _configuration = builder.Build();
    }

    public string ReadString(string key) => _configuration[key];
    public int ReadInt(string key, int defaultValue = 0)
    {
      var valueStr = ReadString(key);
      if (!int.TryParse(valueStr, out var result))
      {
        return defaultValue;
      }
      return result;
    }

    public bool ReadBool(string key, bool defaultValue = false)
    {
      var valueStr = ReadString(key);
      if (!bool.TryParse(valueStr, out var result))
      {
        return defaultValue;
      }
      return result;
    }
  }
}