using Microsoft.EntityFrameworkCore;

namespace store_scrapper_2_int_Tests.Utils
{
  public class AdminContext : DbContext
  {
    public DbSet<PgDatabase> pg_database { get; set; }
    
    private readonly string _connectionString;

    public AdminContext(string connectionString) => _connectionString = connectionString;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(_connectionString);
  }
}