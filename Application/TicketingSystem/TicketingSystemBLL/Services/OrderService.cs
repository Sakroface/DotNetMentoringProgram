using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystemBLL.DTO;
using TicketingSystemBLL.Services.Interfaces;
using TicketingSystemDAL.Entities;
using TicketingSystemDAL.UnitOfWork;

namespace TicketingSystemBLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<EventService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(ILogger<EventService> logger, IMapper mapper, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        public async Task<CartDto> AddSeatToCartAsync(EventSeatDto eventSeatDto)
        {
            try
            {
                if (eventSeatDto is null)
                {
                    throw new ArgumentNullException("OrderService.AddSeatToCartAsync. Dto cannot be null or an empty object.");
                }

                _unitOfWork.BeginTransaction();

                var cart = await _unitOfWork.CartRepository.GetByIdAsync(eventSeatDto.CartId).ConfigureAwait(false);
                var eventSeat = await _unitOfWork.EventSeatRepository.GetByIdAsync(eventSeatDto.SeatId).ConfigureAwait(false);

                cart.EventSeats.Add(eventSeat);
                _unitOfWork.CartRepository.Update(cart);

                _unitOfWork.CommitTransaction();

                var updatedCart = await _unitOfWork.CartRepository.GetByIdAsync(cart.Id).ConfigureAwait(false);
                var dto = _mapper.Map<CartDto>(updatedCart);

                // Calculate total amount for the cart taking price from each seat.
                dto.Amount = dto.EventSeats.Sum(i => i.Price);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _unitOfWork.RollbackTransaction();

                throw;
            }
        }

        public async Task<PaymentDto> BookSeatsAsync(Guid cartId)
        {
            try
            {
                var cart = await _unitOfWork.CartRepository.GetByIdAsync(cartId).ConfigureAwait(false);
                if (cart is null || !cart.EventSeats.Any())
                {
                    return null;
                }

                _unitOfWork.BeginTransaction();

                var seatIds = cart.EventSeats.Select(cs => cs.SeatId).ToList();

                var paymentDto = new PaymentDto
                {
                    Id = Guid.NewGuid(),
                    CartId = cartId,
                    Amount = cart.EventSeats.Sum(cs => cs.Prices.FirstOrDefault(p => p.IsActive).Amount),
                    TimeStamp = DateTime.UtcNow
                };

                var entity = _mapper.Map<Payment>(paymentDto);

                await _unitOfWork.PaymentRepository.InsertAsync(entity).ConfigureAwait(false);

                _unitOfWork.CommitTransaction();

                var cartDto = _mapper.Map<CartDto>(cart);
                var dto = await _paymentService.UpdatePaymentStatusAsync(entity.Id, cartDto, Enums.PaymentStatus.Pending);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"OrderService.BookSeatsAsync. Error: {ex.Message}");
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task CreateCartAsync(CartDto cartDto)
        {
            try
            {
                if (cartDto == null)
                {
                    throw new ArgumentNullException(nameof(cartDto), "OrderService.CreateCartAsync. CartDto cannot be null.");
                }

                _unitOfWork.BeginTransaction();

                if (cartDto.Id == Guid.Empty)
                {
                    cartDto.Id = Guid.NewGuid();
                }

                if (cartDto.EventId <= 0)
                {
                    throw new ArgumentException("EventId must be specified and valid", nameof(cartDto));
                }

                if (cartDto.UserId == Guid.Empty)
                {
                    throw new ArgumentException("UserId must be specified", nameof(cartDto));
                }

                cartDto.Status = Enums.CartStatus.Created;

                if (cartDto.EventSeats == null)
                {
                    cartDto.EventSeats = new List<EventSeatDto>();
                }

                var cartEntity = _mapper.Map<Cart>(cartDto);

                await _unitOfWork.CartRepository.InsertAsync(cartEntity).ConfigureAwait(false);

                _unitOfWork.CommitTransaction();

                _logger.LogInformation($"Cart created successfully with ID: {cartDto.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"OrderService.CreateCartAsync. Error: {ex.Message}");
                _unitOfWork.RollbackTransaction();
                throw;
            }


        }

        public async Task<CartDto> GetCartAsync(Guid cartId)
        {
            try
            {
                var entity = await _unitOfWork.CartRepository.GetByIdAsync(cartId).ConfigureAwait(false);
                if (entity is null)
                {
                    return null;
                }

                var dto = _mapper.Map<CartDto>(entity);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task RemoveSeatFromCartAsync(EventSeatDto eventSeatDto)
        {
            try
            {
                var cart = await _unitOfWork.CartRepository.GetByIdAsync(eventSeatDto.CartId).ConfigureAwait(false);
                if (cart is null)
                {
                    throw new ArgumentException($"Cart with Id: {eventSeatDto.CartId} was not found.");
                }

                var seatToRemove = cart.EventSeats.FirstOrDefault(i => i.EventId == eventSeatDto.EventId && i.SeatId == eventSeatDto.SeatId);
                if (seatToRemove is null)
                {
                    throw new ArgumentException($"Item with EventId {eventSeatDto.EventId} and SeatId {eventSeatDto.SeatId} not found in cart {eventSeatDto.CartId}.");
                }

                _unitOfWork.BeginTransaction();

                cart.EventSeats.Remove(seatToRemove);

                _unitOfWork.CartRepository.Update(cart);

                _unitOfWork.CommitTransaction();

            }
            catch (Exception ex)
            {
                _logger.LogError($"OrderService.RemoveSeatFromCartAsync. Error: {ex.Message}");
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
