using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using store_scrapper_2.DataTransmission.Serialization;

namespace store_scrapper_2.DataTransmission
{
  public class StoreInfoDownloader : IStoreInfoDownloader
  {
    private readonly IUrlDownloader _urlDownloader;

    public StoreInfoDownloader(IUrlDownloader urlDownloader)
    {
      _urlDownloader = urlDownloader;
    }

    public async Task<IEnumerable<StoreInfoResponse>> DownloadAsync(ZipCode request)
    {
      var responseJson = await _urlDownloader.DownloadAsync(request.ToUrl());
      
      var json = responseJson
        .TrimStart('(')
        .TrimEnd(')');

      return GenericJsonSerializer.FromJson<StoresLocatorResponse>(json)
        .ResultData
        .Select(StoreInfoResponse.Create)
        .ToArray();
    }
  }
}