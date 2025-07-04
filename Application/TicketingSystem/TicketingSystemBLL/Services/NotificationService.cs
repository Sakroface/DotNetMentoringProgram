using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystemBLL.DTO;
using TicketingSystemBLL.Enums;
using TicketingSystemBLL.Services.Interfaces;
using TicketingSystemDAL.UnitOfWork;

namespace TicketingSystemBLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageQueueService _messageQueueService;

        public NotificationService(ILogger<NotificationService> logger, IMapper mapper, IUnitOfWork unitOfWork, IMessageQueueService messageQueueService)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _messageQueueService = messageQueueService;
        }

        private async Task SendSeatBookingNotificationAsync(CartDto cart, EventSeatDto eventSeat)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(cart.UserId);
                var eventDetails = await _unitOfWork.EventRepository.GetByIdAsync(eventSeat.EventId);
                var venues = await _unitOfWork.VenueRepository.GetVenuesByEventAsync(cart.EventId);
                var venueSeat = await _unitOfWork.VenueSeatRepository.GetByIdAsync(eventSeat.SeatId);

                var notification = new SeatBookingNotification
                {
                    NotificationTrackingId = Guid.NewGuid(),
                    ActionName = NotificationOperations.TICKET_BOOKING_CONFIRMED,
                    Timestamp = DateTime.UtcNow,
                    Parameters = new NotificationParameters
                    {
                        CustomerEmail = user?.Email,
                        CustomerName = $"{user?.FirstName} {user?.MiddleName} {user?.LastName}",
                        UserId = cart.UserId.ToString()
                    },
                    Content = new NotificationContent
                    {
                        OrderAmount = cart.Amount,
                        OrderSummary = $"Seat {venueSeat.Number} added to cart for {eventDetails?.Name}",
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem
                            {
                                SeatId = eventSeat.Id,
                                SeatNumber = venueSeat.Number.ToString(),
                                Price = eventSeat.Price
                            }
                        },
                        EventName = eventDetails?.Name,
                        EventDate = eventDetails?.StartDate ?? DateTime.MinValue,
                        VenueName = string.Join(",", venues.Select(v => v.Name))
                    }
                };

                await _messageQueueService.PublishSeatBookingNotificationAsync(notification);

                _logger.LogInformation($"Notification queued with tracking ID: {notification.NotificationTrackingId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending seat booking notification");
            }
        }

    }
}
