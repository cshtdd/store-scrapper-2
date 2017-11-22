using System.Threading.Tasks;

namespace store_scrapper_2.DataTransmission
{
  public class StoreInfoDownloader : IStoreInfoDownloader
  {
    private readonly IUrlDownloader _urlDownloader;

    public StoreInfoDownloader(IUrlDownloader urlDownloader)
    {
      _urlDownloader = urlDownloader;
    }

    public async Task<StoreInfoResponse> DownloadAsync(StoreInfoRequest request)
    {
      var responseJson = await _urlDownloader.DownloadAsync(request.ToUrl());
      return StoreInfoResponse.Parse(responseJson);
    }
  }
}