using System.Threading.Tasks;

namespace store_scrapper_2.Services
{
  public interface IBatchDelaySimulator
  {
    Task Delay();
  }
}