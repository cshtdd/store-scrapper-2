using store_scrapper_2.Configuration;
using store_scrapper_2.DAL.Db;

namespace store_scrapper_2.DAL
{
  public class StoreDataContextFactory : IStoreDataContextFactory
  {
    public string ConnectionString { get; }
    public bool LoggingEnabled { get; }

    public StoreDataContextFactory(IConfigurationReader configurationReader)
    {
      ConnectionString = configurationReader.ReadString(ConfigurationKeys.ConnectionStringsStoresDb);
      LoggingEnabled = configurationReader.ReadBool(ConfigurationKeys.EfLogEnabled);
    }

    public StoreDataContext Create() => new StoreDataContext(ConnectionString, LoggingEnabled);
  }
}