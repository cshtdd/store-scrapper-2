using System;

namespace store_scrapper_2.Services
{
  public interface ITimer : IDisposable
  {
    void Start();
    void Stop();
  }
}