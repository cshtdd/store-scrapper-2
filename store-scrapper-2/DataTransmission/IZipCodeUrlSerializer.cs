using store_scrapper_2.Model;

namespace store_scrapper_2.DataTransmission
{
  public interface IZipCodeUrlSerializer
  {
    string ToUrl(ZipCode zipCode);
  }
}