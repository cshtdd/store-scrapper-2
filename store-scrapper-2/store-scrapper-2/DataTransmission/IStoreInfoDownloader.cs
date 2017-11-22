using System.Threading.Tasks;

namespace store_scrapper_2.DataTransmission
{
  public interface IStoreInfoDownloader
  {
    Task<StoreInfoResponse> DownloadAsync(StoreInfoRequest request);
  }
}