using store_scrapper_2.Configuration;

namespace store_scrapper_2_int_Tests.Utils
{
  public abstract class IntegrationTest
  {
    static IntegrationTest() => Mappings.Configure();
  }
}