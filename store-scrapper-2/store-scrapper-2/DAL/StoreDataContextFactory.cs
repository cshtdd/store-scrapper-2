using store_scrapper_2.Configuration;
using store_scrapper_2.DAL.Db;

namespace store_scrapper_2.DAL
{
  public class StoreDataContextFactory : IStoreDataContextFactory
  {
    private readonly IConfigurationReader _configurationReader;
    public string ConnectionString { get; }

    public StoreDataContextFactory(IConfigurationReader configurationReader)
    {
      _configurationReader = configurationReader;
      ConnectionString = _configurationReader.Read(ConfigurationKeys.ConnectionString);
    }

    public StoreDataContext Create() => new StoreDataContext(ConnectionString);
  }
}