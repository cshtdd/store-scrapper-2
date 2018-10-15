using System.Collections.Generic;

using store_scrapper_2.Model;

namespace store_scrapper_2.DataTransmission
{
  public interface IStoreInfoDownloader
  {
    IEnumerable<StoreInfo> Download(ZipCode zipCode);
  }
}