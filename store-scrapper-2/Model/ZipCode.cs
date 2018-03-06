using System;
using System.Linq;
using store_scrapper_2.Logging;

namespace store_scrapper_2.Model
{
  public struct ZipCode
  {
    public string Zip { get; }
    public decimal Latitude { get; }
    public decimal Longitude { get; }

    public ZipCode(string zip, decimal latitude, decimal longitude)
    {
      var zipCode = zip ?? string.Empty;
      
      if (zipCode.Length < 3 || zipCode.Length > 5)
      {
        throw new ArgumentException($"Invalid {nameof(zip)} length");
      }

      if (!zipCode.ToCharArray().All(char.IsDigit))
      {
        throw new ArgumentException($"NonNumeric {nameof(zip)} found");        
      }

      if (latitude < -90m || latitude > 90m)
      {
        throw new ArgumentException($"OutOfRange {nameof(latitude)} found");
      }

      if (longitude < -180m || longitude > 180m)
      {        
        throw new ArgumentException($"OutOfRange {nameof(longitude)} found");
      }
      
      Zip = zipCode;
      Latitude = latitude;
      Longitude = longitude;
    }
    
    public override string ToString() => LogFormatter.Format(new object[]
    {
      nameof(Zip), Zip,
      nameof(Latitude), Latitude.ToString("F8"),
      nameof(Longitude), Longitude.ToString("F8")
    });
  }
}