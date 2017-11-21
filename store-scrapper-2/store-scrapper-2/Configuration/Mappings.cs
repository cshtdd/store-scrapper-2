using AutoMapper;
using store;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2.Configuration
{
  public static class Mappings
  {
    public static void Configure()
    {
      Mapper.Initialize(_ =>
      {
        _.CreateMap<StoreInfoResponse, Store>();
      });      
    }
  }
}