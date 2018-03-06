using System;
using System.Globalization;
using store_scrapper_2;

namespace store_scrapper_2_Tests.Factory
{
  public static class ZipCodeInfoFactory
  {
    public static ZipCodeInfo Create(
      string zip,
      string dateString = "2012-04-23"
    ) => Create(
      zip,
      DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture)
    ); 
    
    public static ZipCodeInfo Create(
      string zip,
      DateTime updateTimeUtc
    ) => new ZipCodeInfo
    {
      ZipCode = ZipCodeFactory.Create(zip),
      UpdateTimeUtc = updateTimeUtc
    };
  }
}