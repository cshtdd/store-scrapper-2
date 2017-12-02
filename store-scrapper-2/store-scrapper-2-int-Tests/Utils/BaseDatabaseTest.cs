using System;
using System.IO;
using store_scrapper_2.Configuration;
using store_scrapper_2.DAL;

namespace store_scrapper_2_int_Tests.Utils
{
  public abstract class BaseDatabaseTest : IDisposable
  {
    private readonly string TestDatabaseName = $"test_stores{Guid.NewGuid():X}.db";
    
    protected IStoreDataContextFactory ContextFactory => new StoreDataContextFactory(TestDatabaseName);
    
    protected bool DatabaseExists => File.Exists(TestDatabaseName);

    static BaseDatabaseTest() => Mappings.Configure();
    
    public void Dispose()
    {
      if (File.Exists(TestDatabaseName))
      {
        File.Delete(TestDatabaseName);
      }
    }
  }
}