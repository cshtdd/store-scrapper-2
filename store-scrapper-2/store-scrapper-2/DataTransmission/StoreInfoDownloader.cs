using System.Threading.Tasks;

namespace store_scrapper_2.DataTransmission
{
  public class StoreInfoDownloader
  {
    private readonly IUrlDownloader _urlDownloader;

    public StoreInfoDownloader(IUrlDownloader urlDownloader)
    {
      _urlDownloader = urlDownloader;
    }

    public async Task<StoreInfoResponse> Download(StoreInfoRequest request)
    {
      return await new Task<StoreInfoResponse>(() => new StoreInfoResponse());
    }
  }
}