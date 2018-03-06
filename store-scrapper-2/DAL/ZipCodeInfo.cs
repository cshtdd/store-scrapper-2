using System;
using store_scrapper_2.Model;

namespace store_scrapper_2
{
  public struct ZipCodeInfo
  {
    public DateTime UpdateTimeUtc { get; set; }
    public ZipCode ZipCode { get; set; }
  }
}