using System;

namespace store_scrapper_2.Instrumentation
{
  public interface IResourcesManager : IDisposable
  {
    void Monitor();
  }
}