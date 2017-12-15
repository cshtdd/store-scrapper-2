using store_scrapper_2.Model;

namespace store_scrapper_2_Tests.Factory
{
  public static class ZipCodeFactory
  {
    public static ZipCode Create(
      string zip = "50001",
      decimal latitude = 12.23m,
      decimal longitude = 100.5m
      ) => new ZipCode(zip, latitude, longitude);
  }
}