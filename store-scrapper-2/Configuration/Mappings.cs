using AutoMapper;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DAL.Db;

namespace store_scrapper_2.Configuration
{
  public static class Mappings
  {
    private static IMapper _mapper;

    public static void Configure()
    {
      var config = new MapperConfiguration(_ =>
      {
        _.AddProfile<DalMappingProfile>();
      });
      
      _mapper = new Mapper(config);
      _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    public static TDestination Map<TDestination>(object source) => _mapper.Map<TDestination>(source);
    public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination) => _mapper.Map(source, destination);

    private class DalMappingProfile : Profile
    {
      public DalMappingProfile()
      {
        CreateMap<StoreInfo, Store>()
          .ForMember(dest => dest.StoreNumber, mi => mi.MapFrom(src => src.StoreNumber.Store.ToString()))
          .ForMember(dest => dest.SatelliteNumber, mi => mi.MapFrom(src => src.StoreNumber.Satellite.ToString()));
      }
    }
  }
}