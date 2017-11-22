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
        _.AddProfile<DalMappingProfile>();
      });
      
      Mapper.AssertConfigurationIsValid();
    }

    class DalMappingProfile : Profile
    {
      public DalMappingProfile()
      {
        CreateMap<StoreInfoResponse, Store>()
          .ForMember(dest => dest.StoreId, mi => mi.Ignore());
      }
    }
  }
}