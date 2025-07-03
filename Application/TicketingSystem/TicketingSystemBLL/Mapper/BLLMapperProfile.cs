using AutoMapper;
using System.Linq;
using TicketingSystemBLL.DTO;
using TicketingSystemDAL.Entities;

namespace TicketingSystemBLL.Mapper
{
    public class BLLMapperProfile : Profile
    {
        public BLLMapperProfile()
        {
            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.EventStatus, opt => opt.MapFrom(src =>
                    src.Status != null ? (Enums.EventStatus)src.Status.Id : default(Enums.EventStatus)));
            CreateMap<EventDto, Event>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.EventStatus))
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.EventVenues, opt => opt.Ignore())
                .ForMember(dest => dest.EventSeats, opt => opt.Ignore());

            CreateMap<Venue, VenueDto>()
                .ForMember(dest => dest.VenueType, opt => opt.MapFrom(src =>
                    src.VenueType != null ? (Enums.VenueType)src.VenueType.Id : default(Enums.VenueType)));
            CreateMap<VenueDto, Venue>()
                .ForMember(dest => dest.VenueTypeId, opt => opt.MapFrom(src => (int)src.VenueType))
                .ForMember(dest => dest.VenueType, opt => opt.Ignore())
                .ForMember(dest => dest.EventVenues, opt => opt.Ignore());

            CreateMap<VenueSection, VenueSectionDto>();
            CreateMap<VenueSectionDto, VenueSection>();

            CreateMap<VenueRow, VenueRowDto>();
            CreateMap<VenueRowDto, VenueRow>();

            CreateMap<VenueSeat, VenueSeatDto>()
                .ForMember(dest => dest.SeatsType, opt => opt.MapFrom(src =>
                    src.SeatsType != null ? (Enums.SeatType)src.SeatsType.Id : default(Enums.SeatType)));
            CreateMap<VenueSeatDto, VenueSeat>()
                .ForMember(dest => dest.SeatsTypeId, opt => opt.MapFrom(src => (int)src.SeatsType));

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    src.Status != null ? (Enums.OrderStatus)src.Status.Id : default(Enums.OrderStatus)));
            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(dest => dest.Cart, opt => opt.Ignore());

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    src.Status != null ? (Enums.CartStatus)src.Status.Id : default(Enums.CartStatus)));
            CreateMap<CartDto, Cart>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    src.Status != null ? (Enums.PaymentStatus)src.Status.Id : default(Enums.PaymentStatus)));
            CreateMap<PaymentDto, Payment>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status));

            CreateMap<EventSeat, EventSeatDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    src.Status != null ? (Enums.EventSeatStatus)src.Status.Id : default(Enums.EventSeatStatus)))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => 
                    src.Price.Amount));
            CreateMap<EventSeatDto, EventSeat>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status));

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Price, PriceDto>();
            CreateMap<PriceDto, Price>();

        }
    }
}
