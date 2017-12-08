using Microsoft.EntityFrameworkCore.Design;
using store_scrapper_2.Configuration;
using store_scrapper_2.DAL;
using store_scrapper_2.DAL.Db;

namespace store_scrapper_2
{
  /// <summary>
  /// # What is this?
  ///   A class that will get invoked by the `dotnet ef` tool to construct the StoreDataContext
  /// 
  /// # How did you learn about it?
  ///   https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet
  ///  
  /// # How to create a migration? 
  /// > dotnet ef migrations add RemoveListingNumber
  /// 
  /// </summary>
  // ReSharper disable once UnusedMember.Global
  internal class ProgramEfCli : IDesignTimeDbContextFactory<StoreDataContext>
  {
    public ProgramEfCli() => IocContainer.Initialize();

    public StoreDataContext CreateDbContext(string[] args) => IocContainer.Resolve<IStoreDataContextFactory>().Create();
  }
}