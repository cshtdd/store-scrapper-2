namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyStatistics
  {
    public ProxyInfo Proxy { get; }
      
    public int SuccessThreshold { get; }
    public int FailedThreshold { get; }

    public int SuccessCount { get; set; }
    public int FailedCount { get; set; }

    public ProxyStatistics(ProxyInfo proxy, int successThreshold, int failedThreshold)
    {
      Proxy = proxy;

      SuccessCount = 0;
      SuccessThreshold = successThreshold;

      FailedCount = 0;
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