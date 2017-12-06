using Microsoft.Extensions.Configuration;

namespace store_scrapper_2.DAL
{
  public class ConnectionStringReader : IConnectionStringReader
  {
    public string EnvironmentName { get; }

    public ConnectionStringReader() => EnvironmentName = "PROD";
    public ConnectionStringReader(string environmentName) => EnvironmentName = environmentName;

    public string Read()
    {
      var builder = new ConfigurationBuilder()
        .AddJsonFile("config.json", true)
        .AddJsonFile($"config.{EnvironmentName}.json", true)
        .AddEnvironmentVariables();
      var configuration = builder.Build();

      return configuration["StoresDb:ConnectionString"];
    }
  }
}