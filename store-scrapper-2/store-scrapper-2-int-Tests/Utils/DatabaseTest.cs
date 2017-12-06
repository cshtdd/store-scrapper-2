using System;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2.DAL;

namespace store_scrapper_2_int_Tests.Utils
{
  public abstract class DatabaseTest : IntegrationTest, IDisposable
  {
    private const string TestDatabaseName = "test_stores";
    private readonly string _connectionStringTestDb = $"Server=localhost;Database={TestDatabaseName}";

    protected IStoreDataContextFactory ContextFactory => new StoreDataContextFactory(_connectionStringTestDb);

    public DatabaseTest() => DeleteDatabaseIfNeeded();

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