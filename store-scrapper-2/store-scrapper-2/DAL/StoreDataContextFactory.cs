using store_scrapper_2.DAL.Db;

namespace store_scrapper_2.DAL
{
  public class StoreDataContextFactory : IStoreDataContextFactory
  {
    private readonly IConnectionStringReader _connectionStringReader;
    public string ConnectionString { get; }

    public StoreDataContextFactory(IConnectionStringReader connectionStringReader)
    {
      _connectionStringReader = connectionStringReader;
      ConnectionString = _connectionStringReader.Read();
    }

    public StoreDataContext Create() => new StoreDataContext(ConnectionString);
  }
}