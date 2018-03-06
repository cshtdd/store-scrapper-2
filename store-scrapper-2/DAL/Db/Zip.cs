using System;
using System.ComponentModel.DataAnnotations;

namespace store_scrapper_2.DAL.Db
{
  public class Zip
  {
    [Key]
    public string ZipCode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public DateTime? UpdateTimeUtc { get; set; }
  }
}