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

    public async Task<IEnumerable<StoreInfo>> DownloadAsync(ZipCode zipCode)
    {
      var responseJson = await _urlDownloader.DownloadAsync(zipCode.ToUrl());
      
      var json = responseJson
        .TrimStart('(')
        .TrimEnd(')');

      return GenericJsonSerializer.FromJson<StoresLocatorResponse>(json)
        .ResultData
        .Select(StoreInfo.Create)
        .ToArray();
    }
  }
}