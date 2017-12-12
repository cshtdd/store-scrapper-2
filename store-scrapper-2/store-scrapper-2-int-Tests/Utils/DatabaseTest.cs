using System;
using store_scrapper_2;
using store_scrapper_2.Configuration;
using store_scrapper_2.DAL;

namespace store_scrapper_2_int_Tests.Utils
{
  public abstract class DatabaseTest : IntegrationTest, IDisposable
  {
    private static ConfigurationReader ConfigurationReader { get; } = new ConfigurationReader("TEST");
    protected static IStoreDataContextFactory ContextFactory => new StoreDataContextFactory(ConfigurationReader);

    protected DatabaseTest() => DeleteDatabaseIfNeeded();

    void IDisposable.Dispose() => DeleteDatabaseIfNeeded();

    private void DeleteDatabaseIfNeeded()
    {
      using (var context = ContextFactory.Create())
      {
        context.Database.EnsureDeleted();
      }
    }

    protected IPersistenceInitializer CreatePersistenceInitializer()
    {
      return new PersistenceInitializer(ContextFactory, new ZipCodesSeeder(ContextFactory, ConfigurationReader));
    }
  }
}