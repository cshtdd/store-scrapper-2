namespace store_scrapper_2.DAL
{
  public class StoreDataContextFactory : IStoreDataContextFactory
  {
    public string ConnectionString { get; }

    public StoreDataContextFactory(string connectionString) => ConnectionString = connectionString;

    public StoreDataContext Create() => new StoreDataContext(ConnectionString);
  }
}