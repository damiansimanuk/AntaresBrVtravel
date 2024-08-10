namespace Antares.VTravel.UI.Mapper;

using Antares.VTravel.Client.Dto;
using Antares.VTravel.Shared; 
using Antares.VTravel.UI.Data;
using AutoMapper;

public class MapperService
{
    IMapper mapper;

    public MapperService()
    {
        var config = new MapperConfiguration(conf =>
        {
            conf.CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>));
            conf.CreateMap<Tour, TourDto>().ReverseMap();
        });

        mapper = config.CreateMapper();
    }

    public TourDto ToDto(Tour e) => mapper.Map<TourDto>(e);
}
