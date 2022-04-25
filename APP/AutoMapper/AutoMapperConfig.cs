

using APP.ViewModels;
using APP.Business.Models;
using AutoMapper;

namespace APP.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Flight_Schedule, Flight_ScheduleViewModels>()
          .ForMember(d => d.airline, o => o.MapFrom(s => s.airlineOBJ.Name))
          .ForMember(d => d.flight, o => o.MapFrom(s => s.flightOBJ.cod))
          .ForMember(d => d.airport, o => o.MapFrom(s => s.airportOBJ.Name))
          .ReverseMap();
        }
    }
}