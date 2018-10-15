using System.Collections.Generic;


namespace store_scrapper_2
{
  public interface IZipCodeDataService
  {
    IEnumerable<ZipCodeInfo> All();
    void UpdateZipCode(string zipCode);
  }
}