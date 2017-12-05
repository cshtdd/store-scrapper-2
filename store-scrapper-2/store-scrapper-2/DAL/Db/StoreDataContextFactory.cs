namespace store_scrapper_2.DAL
{
  public class StoreDataContextFactory : IStoreDataContextFactory
  {
    public string DatabaseName { get; }
    public SupportedDatabases DatabaseProvider { get; }
    public string ConnectionString { get; }

    public StoreDataContextFactory(string databaseName = "stores.db", SupportedDatabases databaseProvider = SupportedDatabases.Sqlite)
    {
      DatabaseName = databaseName;
      DatabaseProvider = databaseProvider;
      ConnectionString = $"Data Source={DatabaseName}";
    }

    public StoreDataContext Create() => new StoreDataContext(ConnectionString, DatabaseProvider);
  }
}