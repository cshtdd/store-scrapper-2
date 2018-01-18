using store_scrapper_2.Configuration;
using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public class StorePersistenceCalculator : IStorePersistenceCalculator
  {
    private readonly ICacheWithExpiration _cacheWithExpiration;
    private readonly uint _expirationMs;

    public StorePersistenceCalculator(ICacheWithExpiration cacheWithExpiration, IConfigurationReader configurationReader)
    {
      _cacheWithExpiration = cacheWithExpiration;
      _expirationMs = configurationReader.ReadUInt(ConfigurationKeys.StoresWriteCacheExpirationMs);
    }

    public bool WasPersistedRecently(StoreNumber storeNumber) => _cacheWithExpiration.Contains(storeNumber.ToString());

    public void PreventFuturePersistence(StoreNumber storeNumber) => _cacheWithExpiration.Add(storeNumber.ToString(), _expirationMs);
  }
}