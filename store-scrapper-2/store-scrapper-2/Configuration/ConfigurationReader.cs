using Microsoft.Extensions.Configuration;

namespace store_scrapper_2.Configuration
{
  public class ConfigurationReader : IConfigurationReader
  {
    public string EnvironmentName { get; }

    public ConfigurationReader() => EnvironmentName = "PROD";
    public ConfigurationReader(string environmentName) => EnvironmentName = environmentName;
    
    public string Read(string key)
    {
      var builder = new ConfigurationBuilder()
        .AddJsonFile("config.json", true)
        .AddJsonFile($"config.{EnvironmentName}.json", true)
        .AddEnvironmentVariables();
      var configuration = builder.Build();

      return configuration[key];
    }
  }
}