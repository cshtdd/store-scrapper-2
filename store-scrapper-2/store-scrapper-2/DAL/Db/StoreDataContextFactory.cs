namespace store_scrapper_2.DAL
{
  public class StoreDataContextFactory : IStoreDataContextFactory
  {
    public SupportedDatabases DatabaseProvider { get; }
    public string ConnectionString { get; }

    public StoreDataContextFactory(string connectionString, SupportedDatabases databaseProvider)
    {
      DatabaseProvider = databaseProvider;
      ConnectionString = connectionString;
    }

    public StoreDataContext Create() => new StoreDataContext(ConnectionString, DatabaseProvider);
  }
}