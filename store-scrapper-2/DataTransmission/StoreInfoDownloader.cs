using System.Collections.Generic;
using System.Linq;

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

    public IEnumerable<StoreInfo> Download(ZipCode zipCode)
    {
      var url = _zipCodeUrlSerializer.ToUrl(zipCode);
      var responseJson = _urlDownloader.Download(url);
      
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