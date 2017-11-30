namespace store_scrapper_2.DAL
{
  public class StoreDataContextFactory : IStoreDataContextFactory
  {
    public string DatabaseName { get; }

    public StoreDataContextFactory(string databaseName = "stores.db") => DatabaseName = databaseName;

    public StoreDataContext Create() => new StoreDataContext(DatabaseName);
  }
}