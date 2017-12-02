using System;
using System.IO;
using store_scrapper_2.DAL;

namespace store_scrapper_2_int_Tests.Utils
{
  public abstract class BaseDatabaseTest : IDisposable
  {
    private const string TestDatabaseName = "test_stores.db";
    
    protected readonly IStoreDataContextFactory ContextFactory = new StoreDataContextFactory(TestDatabaseName);
    
    protected bool DatabaseExists => File.Exists(TestDatabaseName);
    
    public void Dispose()
    {
      if (File.Exists(TestDatabaseName))
      {
        File.Delete(TestDatabaseName);
      }
    }
  }
}