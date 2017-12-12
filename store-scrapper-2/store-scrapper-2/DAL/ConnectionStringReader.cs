using store_scrapper_2.Configuration;

namespace store_scrapper_2.DAL
{
  public class ConnectionStringReader : IConnectionStringReader
  {
    private readonly IConfigurationReader _configurationReader;

    public ConnectionStringReader(IConfigurationReader configurationReader) => _configurationReader = configurationReader;

    public string Read() => _configurationReader.Read("StoresDb:ConnectionString");
  }
}