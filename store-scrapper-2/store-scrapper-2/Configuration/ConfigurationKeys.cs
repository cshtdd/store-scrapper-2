using System;

namespace store_scrapper_2.Configuration
{
  public static class ConfigurationKeys
  {
    public const string ConnectionStringsStoresDb = "ConnectionStrings:StoresDb";
    public const string EfLogEnabled = "Ef:LogEnabled";
    public const string SeedsZipsFilename = "Seeds:ZipsFilename";
    [Obsolete]
    public const string ZipCodesBatchSize = "ZipCodes:BatchSize";
    public const string ZipCodesDelayMs = "ZipCodes:DelayMs";
    [Obsolete]
    public const string ZipCodesMaxBatchCount = "ZipCodes:MaxBatchCount";
    public const string ZipCodesRunContinuosly = "ZipCodes:RunContinuosly";
    public const string StoresWriteCacheExpirationMs = "Stores:WriteCacheExpirationMs";
  }
}