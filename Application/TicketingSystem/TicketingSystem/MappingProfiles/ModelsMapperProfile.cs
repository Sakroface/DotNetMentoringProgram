using AutoMapper;
using TicketingSystem.Models;
using TicketingSystemBLL.DTO;
using TicketingSystemBLL.Enums;

namespace TicketingSystem.MappingProfiles
{
    public class ModelsMapperProfile : Profile
    { 
        public ModelsMapperProfile()
        { 
            CreateMap<EventModel, EventDto>();
            CreateMap<EventDto, EventModel>();

            CreateMap<VenueDto, VenueModel>();
            CreateMap<VenueModel, VenueDto>();

            CreateMap<VenueModel, VenueDto>()
                .ForMember(dest => dest.VenueType, opt => opt.MapFrom(src => (VenueType)src.TypeId));
            CreateMap<VenueDto, VenueModel>()
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => (int)src.VenueType));

            CreateMap<VenueSectionModel, VenueSectionDto>();
            CreateMap<VenueSectionDto, VenueSectionModel>();

            CreateMap<VenueRowModel, VenueRowDto>();
            CreateMap<VenueRowDto, VenueRowModel>();

            CreateMap<VenueSeatModel, VenueSeatDto>()
                .ForMember(dest => dest.SeatsType, opt => opt.MapFrom(src => (SeatType)src.SeatsTypeId));
            CreateMap<VenueSeatDto, VenueSeatModel>()
                .ForMember(dest => dest.SeatsTypeId, opt => opt.MapFrom(src => (int)src.SeatsType));

            CreateMap<OrderModel, OrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (OrderStatus)src.StatusId));
            CreateMap<OrderDto, OrderModel>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status));

            CreateMap<CartModel, CartDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (CartStatus)src.StatusId));
            CreateMap<CartDto, CartModel>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status));

            CreateMap<PaymentModel, PaymentDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (PaymentStatus)src.StatusId));
            CreateMap<PaymentDto, PaymentModel>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status));

            CreateMap<EventSeatModel, EventSeatDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (EventSeatStatus)src.StatusId));
            CreateMap<EventSeatDto, EventSeatModel>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status));

            CreateMap<UserModel, UserDto>();
            CreateMap<UserDto, UserModel>();

            CreateMap<PriceModel, PriceDto>();
            CreateMap<PriceDto, PriceModel>();
        }
    }
}
