using System.Collections.Generic;
using System.Threading.Tasks;

namespace store_scrapper_2.DataTransmission
{
  public interface IStoreInfoDownloader
  {
    Task<IEnumerable<StoreInfo>> DownloadAsync(ZipCode zipCode);
  }
}