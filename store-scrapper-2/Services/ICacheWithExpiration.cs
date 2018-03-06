namespace store_scrapper_2.Services
{
  public interface ICacheWithExpiration
  {
    bool Contains(string key);
    void Add(string key, uint expirationMs);
  }
}