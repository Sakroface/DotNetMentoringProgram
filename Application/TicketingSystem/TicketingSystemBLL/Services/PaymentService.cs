using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystemBLL.DTO;
using TicketingSystemBLL.Enums;
using TicketingSystemBLL.Services.Interfaces;
using TicketingSystemDAL.UnitOfWork;

namespace TicketingSystemBLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<EventService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IEventSeatService _eventSeatService;

        public PaymentService(ILogger<EventService> logger, IMapper mapper, IUnitOfWork unitOfWork, IOrderService orderService, IEventSeatService eventSeatService)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _eventSeatService = eventSeatService;
        }

        public async Task<PaymentDto> UpdatePaymentStatusAsync(Guid paymentId, PaymentStatus status)
        {
            try
            {
                if (paymentId == Guid.Empty)
                {
                    throw new ArgumentNullException("PaymentService.UpdatePaymentStatusAsync. Payment id cannot be an empty Guid.");
                }

                _unitOfWork.BeginTransaction();

                var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId).ConfigureAwait(false);

                payment.StatusId = (int)status;
                _unitOfWork.PaymentRepository.Update(payment);

                var seatStatus = GetSeatStatusFromPaymentStatus(status);

                var cart = await _orderService.GetCartAsync(payment.CartId).ConfigureAwait(false);

                await _eventSeatService.UpdateEventSeatsStatusAsync(cart.EventSeats.Select(es => es.Id), seatStatus).ConfigureAwait(false);

                _unitOfWork.CommitTransaction();

                var entity = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId).ConfigureAwait(false);

                return _mapper.Map<PaymentDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _unitOfWork.RollbackTransaction();

                throw;
            }
        }

        public async Task<PaymentDto> GetPaymentAsync(Guid paymentId)
        {
            try
            {
                if (paymentId == Guid.Empty)
                {
                    throw new ArgumentNullException("PaymentService.UpdatePaymentStatusAsync. Payment id cannot be an empty Guid.");
                }

                var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId).ConfigureAwait(false);

                return _mapper.Map<PaymentDto>(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _unitOfWork.RollbackTransaction();

                throw;
            }
        }

        private EventSeatStatus GetSeatStatusFromPaymentStatus(PaymentStatus status)
        {
            try
            {
                return status switch
                {
                    (PaymentStatus.Completed) => EventSeatStatus.Sold,
                    (PaymentStatus.Failed) => EventSeatStatus.Available,
                    (PaymentStatus.Pending) => EventSeatStatus.Booked,
                    _ => throw new Exception("PaymentService.GetSeatStatusFromPaymentStatus. Cannot map payment status to seat status. Option not provided."),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
