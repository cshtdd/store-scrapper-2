namespace store_scrapper_2.DAL
{
  public class StoreDataContextFactorySqlite : StoreDataContextFactory
  {
    public string DatabaseName { get; }
    
    public StoreDataContextFactorySqlite(string databaseName = "stores.db") : base($"Data Source={databaseName}", SupportedDatabases.Sqlite)
    {
      DatabaseName = databaseName;
    }
  }
}