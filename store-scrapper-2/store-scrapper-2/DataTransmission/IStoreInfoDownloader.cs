using System.Collections.Generic;
using System.Threading.Tasks;
using store_scrapper_2.Model;

namespace store_scrapper_2.DataTransmission
{
  public interface IStoreInfoDownloader
  {
    Task<IEnumerable<StoreInfo>> DownloadAsync(ZipCode zipCode);
  }
}