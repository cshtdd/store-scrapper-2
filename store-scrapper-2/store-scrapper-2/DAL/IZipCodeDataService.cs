using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2
{
  public interface IZipCodeDataService
  {
    Task<ZipCode> ReadAsync(string zipCode);
  }
}