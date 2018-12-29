using System;

namespace store_scrapper_2.Services
{
  public interface ITimerFactory
  {
    ITimer Create(Action timerElapsedDelegate, uint intervalMs);
    ITimer LastCreated { get; }
  }
}