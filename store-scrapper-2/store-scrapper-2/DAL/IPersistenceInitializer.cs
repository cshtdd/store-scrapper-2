using System.Threading.Tasks;

namespace store_scrapper_2
{
  public interface IPersistenceInitializer
  {
    Task Initialize();
  }
}