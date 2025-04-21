using AutoMapper;
using TicketingSystemDAL.Entities;

namespace TicketingSystemBLL.Mapper
{
    public class BLLMapperProfile : Profile
    {
        public BLLMapperProfile()
        {
            CreateMap<Objects.Event, Event>();
            CreateMap<Event, Objects.Event>();

            CreateMap<Objects.Venue, Venue>();
            CreateMap<Venue, Objects.Venue>();
        }
    }
}
