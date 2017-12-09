using AutoMapper;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DAL.Db;

namespace store_scrapper_2.Configuration
{
  public static class Mappings
  {
    public static void Configure()
    {
      Mapper.Initialize(_ =>
      {
        _.AddProfile<DalMappingProfile>();
      });
      
      Mapper.AssertConfigurationIsValid();
    }

    private class DalMappingProfile : Profile
    {
      public DalMappingProfile()
      {
        CreateMap<StoreInfo, Store>()
          .ForMember(dest => dest.StoreId, mi => mi.Ignore())
          .ForMember(dest => dest.StoreNumber, mi => mi.MapFrom(src => src.StoreNumber.Store.ToString()))
          .ForMember(dest => dest.SatelliteNumber, mi => mi.MapFrom(src => src.StoreNumber.Satellite.ToString()));
      }
    }
  }
}