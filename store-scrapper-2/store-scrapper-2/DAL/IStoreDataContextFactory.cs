using store_scrapper_2.DAL.Db;

namespace store_scrapper_2.DAL
{
  public interface IStoreDataContextFactory
  {
    StoreDataContext Create();
  }
}