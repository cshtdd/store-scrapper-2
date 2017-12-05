using System;
using System.IO;
using store_scrapper_2.DAL;

namespace store_scrapper_2_int_Tests.Utils
{
  public abstract class DatabaseTest : IntegrationTest, IDisposable
  {
    private readonly string _testDatabaseName = $"test_stores{Guid.NewGuid():X}.db";
    
    protected IStoreDataContextFactory ContextFactory => new StoreDataContextFactory(_testDatabaseName);
    
    protected bool DatabaseExists => File.Exists(_testDatabaseName);
    
    public void Dispose()
    {
      if (File.Exists(_testDatabaseName))
      {
        File.Delete(_testDatabaseName);
      }
    }
  }
}