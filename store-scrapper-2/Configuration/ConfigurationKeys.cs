namespace store_scrapper_2.Configuration
{
  public static class ConfigurationKeys
  {
    public const string ConnectionStringsStoresDb = "ConnectionStrings:StoresDb";
    public const string EfLogEnabled = "Ef:LogEnabled";
    public const string SeedsZipsFilename = "Seeds:ZipsFilename";
    public const string ZipCodesDelayMs = "ZipCodes:DelayMs";
    public const string ZipCodesRunContinuosly = "ZipCodes:RunContinuosly";
    public const string StoresWriteCacheExpirationMs = "Stores:WriteCacheExpirationMs";
    public const string ProxyTimeoutMs = "Proxy:TimeoutMs";
    public const string ProxyFailThreshold = "Proxy:FailThreshold";
    public const string ProxyMaxCount = "Proxy:MaxCount";
    public const string ProxyUrlMaxAttempts = "Proxy:UrlMaxAttempts";
    public const string DeadlockDetectionEnabled = "DeadlockDetection:Enabled";
    public const string DeadlockDetectionTimeoutMs = "DeadlockDetection:TimeoutMs";
  }
}