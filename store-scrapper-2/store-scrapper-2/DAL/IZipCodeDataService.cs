using System.Collections.Generic;
using System.Threading.Tasks;

namespace store_scrapper_2
{
  public interface IZipCodeDataService
  {
    Task<IEnumerable<ZipCodeInfo>> AllAsync();
    Task UpdateZipCodeAsync(string zipCode);
  }
}