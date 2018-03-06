using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public interface IStorePersistenceCalculator
  {
    bool WasPersistedRecently(StoreNumber storeNumber);
    void PreventFuturePersistence(StoreNumber storeNumber);
  }
}
