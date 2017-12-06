using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace store_scrapper_2_int_Tests.Utils
{
  public class PgDatabase
  {
    [Key]
    [Column("datname")]
    public string DatabaseName { get; set; }
  }
}