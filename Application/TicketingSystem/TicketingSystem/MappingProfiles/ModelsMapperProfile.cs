using AutoMapper;
using TicketingSystem.Models;
using TicketingSystemBLL.Objects;

namespace TicketingSystem.MappingProfiles
{
    public class ModelsMapperProfile : Profile
    {
        public ModelsMapperProfile()
        { 
            CreateMap<EventModel, Event>();
            CreateMap<Event, EventModel>();

            CreateMap<Event, EventModel>();
            CreateMap<Event, EventModel>();
        }
    }
}
