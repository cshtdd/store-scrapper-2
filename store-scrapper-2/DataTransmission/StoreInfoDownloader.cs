using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using store_scrapper_2.Model;
using store_scrapper_2.DataTransmission.Serialization;

namespace store_scrapper_2.DataTransmission
{
  public class StoreInfoDownloader : IStoreInfoDownloader
  {
    private readonly IUrlDownloader _urlDownloader;
    private readonly IZipCodeUrlSerializer _zipCodeUrlSerializer;

    public StoreInfoDownloader(IUrlDownloader urlDownloader, IZipCodeUrlSerializer zipCodeUrlSerializer)
    {
      _urlDownloader = urlDownloader;
      _zipCodeUrlSerializer = zipCodeUrlSerializer;
    }

    public async Task<IEnumerable<StoreInfo>> DownloadAsync(ZipCode zipCode)
    {
      var url = _zipCodeUrlSerializer.ToUrl(zipCode);
      var responseJson = await _urlDownloader.DownloadAsync(url);
      
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