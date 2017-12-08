using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2.Services
{
  public interface ISingleStorePersistor
  {
    Task PersistAsync(StoreInfoResponse store);
  }
}