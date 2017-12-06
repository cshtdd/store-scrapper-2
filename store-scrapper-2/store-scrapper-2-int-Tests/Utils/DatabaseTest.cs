using System;
using store_scrapper_2.DAL;

namespace store_scrapper_2_int_Tests.Utils
{
  public abstract class DatabaseTest : IntegrationTest, IDisposable
  {
    protected static IStoreDataContextFactory ContextFactory => new StoreDataContextFactory(new ConnectionStringReader("TEST"));

    protected DatabaseTest() => DeleteDatabaseIfNeeded();

    void IDisposable.Dispose() => DeleteDatabaseIfNeeded();

    private void DeleteDatabaseIfNeeded()
    {
      using (var context = ContextFactory.Create())
      {
        context.Database.EnsureDeleted();
      }
    }
  }
}