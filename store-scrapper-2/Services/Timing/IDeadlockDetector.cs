namespace store_scrapper_2.Services
{
  public interface IDeadlockDetector
  {
    void Init();
    void UpdateStatus();
  }
}