namespace store_scrapper_2.DAL.Db
{
  public class Zip
  {
    public int ZipId { get; set; }
    
    public string ZipCode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
  }
}