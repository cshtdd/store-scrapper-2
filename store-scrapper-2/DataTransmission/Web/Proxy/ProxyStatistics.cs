namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyStatistics
  {
    public ProxyInfo Proxy { get; }
      
    public uint SuccessThreshold { get; }
    public uint FailedThreshold { get; }

    public uint SuccessCount { get; set; }
    public uint FailedCount { get; set; }

    public ProxyStatistics(ProxyInfo proxy,
      uint successThreshold, uint failedThreshold, 
      uint successCount = 0, uint failedCount = 0)
    {
      Proxy = proxy;

      SuccessCount = successCount;
      SuccessThreshold = successThreshold;

      FailedCount = failedCount;
      FailedThreshold = failedThreshold;
    }
    

    public bool HasBeenUsedTooMuch => SuccessCount >= SuccessThreshold ||
                                      FailedCount >= FailedThreshold;

    public ProxyStatistics IncrementFailedCount()
    {
      FailedCount++;
      return this;
    }
      
    public ProxyStatistics IncrementSuccessCount()
    {
      SuccessCount++;
      return this;
    }
  }
}