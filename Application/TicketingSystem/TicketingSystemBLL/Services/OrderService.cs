using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
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

                var cart = await _unitOfWork.CartRepository.GetByIdAsync(eventSeatDto.CartId).ConfigureAwait(false);
                var eventSeat = await _unitOfWork.EventSeatRepository.GetByIdAsync(eventSeatDto.SeatId).ConfigureAwait(false);

                if (eventSeat is null)
                {
                    _logger.LogInformation($"Could not retrieve seat with the Id: {eventSeatDto.SeatId}");
                    return null;
                }

                if (cart.EventSeats is null)
                    cart.EventSeats = new List<EventSeat>();

                cart.EventSeats.Add(eventSeat);
                _logger.LogInformation($"User {cart.UserId} has added seat {eventSeat.Id} to cart {cart.Id}.");

                _unitOfWork.BeginTransaction();

                _unitOfWork.CartRepository.Update(cart);

                _unitOfWork.CommitTransaction();

                var updatedCart = await _unitOfWork.CartRepository.GetCartWithSeatsAsync(cart.Id).ConfigureAwait(false);
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

        #region Pessimistic concurrency approach

        public async Task<CartDto> AddSeatToCartWithPessimisticConcurrencyAsync(EventSeatDto eventSeatDto)
        {
            try
            {
                if (eventSeatDto is null)
                {
                    throw new ArgumentNullException(nameof(eventSeatDto));
                }

                try
                {
                    var eventSeat = await _unitOfWork.EventSeatRepository.GetByIdAsync(eventSeatDto.SeatId);

                    if (eventSeat is null)
                    {
                        _logger.LogInformation($"Could not retrieve seat with the Id: {eventSeatDto.SeatId}");
                        return null;
                    }

                    // Check if seat is already reserved
                    if (eventSeat.StatusId == (int)Enums.EventSeatStatus.Booked)
                    {
                        _logger.LogInformation($"Seat {eventSeatDto.SeatId} is already booked.");
                        return null;
                    }

                    if (eventSeat.StatusId == (int)Enums.EventSeatStatus.Available)
                    {
                        // Transaction with serializable isolation level to lock the retrieved row.
                        await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable);

                        var cart = await _unitOfWork.CartRepository.GetCartWithSeatsAsync(eventSeatDto.CartId);

                        if (cart != null && cart.EventSeats.Any(es => es.SeatId == eventSeat.Id))
                        {
                            _logger.LogInformation($"Seat {eventSeatDto.SeatId} is already in cart {eventSeatDto.CartId}.");
                            return null;
                        }

                        // Reserve the seat
                        eventSeat.StatusId = (int)Enums.EventSeatStatus.Booked;
                        eventSeat.CartId = cart.Id;
                        eventSeat.BookedAt = DateTime.Now;

                        if (cart.EventSeats is null)
                            cart.EventSeats = new List<EventSeat>();

                        cart.EventSeats.Add(eventSeat);

                        _unitOfWork.CartRepository.Update(cart);

                        _unitOfWork.CommitTransaction();

                        _logger.LogInformation($"User {cart.UserId} successfully added seat {eventSeat.Id} to cart {cart.Id}.");
                    }

                    
                    var updatedCart = await _unitOfWork.CartRepository.GetCartWithSeatsAsync(eventSeatDto.CartId);
                    var dto = _mapper.Map<CartDto>(updatedCart);
                    dto.Amount = dto.EventSeats.Sum(i => i.Price);

                    return dto;
                }
                catch
                {
                    _unitOfWork.RollbackTransaction();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddSeatToCartWithSerializableIsolationAsync: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Optimistic concurrency approach

        public async Task<CartDto> AddSeatToCartWithOptimisticConcurrencyAsync(EventSeatDto eventSeatDto)
        {
            try
            {
                if (eventSeatDto is null)
                {
                    throw new ArgumentNullException(nameof(eventSeatDto));
                }

                try
                {
                    // Transaction with read committed to get the row with the latest state.
                    await _unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted);

                    var eventSeat = await _unitOfWork.EventSeatRepository.GetByIdAsync(eventSeatDto.SeatId);

                    if (eventSeat is null)
                    {
                        _logger.LogInformation($"Could not retrieve seat with the Id: {eventSeatDto.SeatId}");
                        return null;
                    }

                    // Check if seat is already reserved
                    if (eventSeat.StatusId == (int)Enums.EventSeatStatus.Booked)
                    {
                        _logger.LogInformation($"Seat {eventSeatDto.SeatId} is already booked.");
                        return null;
                    }


                    if (eventSeat.StatusId == (int)Enums.EventSeatStatus.Available)
                    {
                        var cart = await _unitOfWork.CartRepository.GetCartWithSeatsAsync(eventSeatDto.CartId);

                        if (cart != null && cart.EventSeats.Any(es => es.SeatId == eventSeat.Id))
                        {
                            _logger.LogInformation($"Seat {eventSeatDto.SeatId} is already in cart {eventSeatDto.CartId}.");
                            return null;
                        }

                        //Double check if status changed for the seat.
                        eventSeat = await _unitOfWork.EventSeatRepository.GetByIdAsync(eventSeatDto.SeatId);

                        if (eventSeat.StatusId == (int)Enums.EventSeatStatus.Booked)
                        {
                            _logger.LogInformation($"Seat {eventSeatDto.SeatId} is already booked.");
                            return null;
                        }

                        // Reserve the seat
                        eventSeat.StatusId = (int)Enums.EventSeatStatus.Booked;
                        eventSeat.CartId = cart.Id;
                        eventSeat.BookedAt = DateTime.Now;

                        if (cart.EventSeats is null)
                            cart.EventSeats = new List<EventSeat>();

                        cart.EventSeats.Add(eventSeat);

                        _unitOfWork.CartRepository.Update(cart);

                        _logger.LogInformation($"User {cart.UserId} successfully added seat {eventSeat.Id} to cart {cart.Id}.");
                    }

                    _unitOfWork.CommitTransaction();

                    var updatedCart = await _unitOfWork.CartRepository.GetCartWithSeatsAsync(eventSeatDto.CartId);
                    var dto = _mapper.Map<CartDto>(updatedCart);
                    dto.Amount = dto.EventSeats.Sum(i => i.Price);

                    return dto;
                }
                catch
                {
                    _unitOfWork.RollbackTransaction();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddSeatToCartWithSerializableIsolationAsync: {ex.Message}");
                throw;
            }
        }
        #endregion

        public async Task<PaymentDto> BookSeatsAsync(Guid cartId)
        {
            try
            {
                PaymentDto paymentDto = null;

                var cart = await _unitOfWork.CartRepository.GetCartWithSeatsAsync(cartId).ConfigureAwait(false);
                if (cart is null || cart.EventSeats is null || cart.EventSeats.Count == 0)
                {
                    return null;
                }

                _unitOfWork.BeginTransaction();

                var seatIds = cart.EventSeats.Select(cs => cs.SeatId).ToList();

                var payment = await _unitOfWork.PaymentRepository.GetPaymentByCartIdAsync(cart.Id).ConfigureAwait(false);

                if (payment is null)
                {
                    paymentDto = new PaymentDto
                    {
                        CartId = cartId,
                        Amount = cart.EventSeats.Sum(cs => cs.Price.Amount),
                        TimeStamp = DateTime.UtcNow,
                        Status = Enums.PaymentStatus.Pending
                    };

                    var entity = _mapper.Map<Payment>(paymentDto);

                    await _unitOfWork.PaymentRepository.InsertAsync(entity).ConfigureAwait(false);

                    _unitOfWork.CommitTransaction();

                    paymentDto.Id = entity.Id; 
                }
                else
                {
                    paymentDto = _mapper.Map<PaymentDto>(payment);
                }

                return paymentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"OrderService.BookSeatsAsync. Error: {ex.Message}");
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<Guid> CreateCartAsync(CartDto cartDto)
        {
            try
            {
                if (cartDto == null)
                {
                    throw new ArgumentNullException(nameof(cartDto), "OrderService.CreateCartAsync. CartDto cannot be null.");
                }


                if (cartDto.Id == Guid.Empty)
                {
                    cartDto.Id = Guid.NewGuid();
                }

                var activeCart = await _unitOfWork.CartRepository.GetActiveCartByUserId(cartDto.UserId);

                if (activeCart != null)
                {
                    return activeCart.Id;
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

                _unitOfWork.BeginTransaction();

                await _unitOfWork.CartRepository.InsertAsync(cartEntity).ConfigureAwait(false);

                _unitOfWork.CommitTransaction();

                _logger.LogInformation($"Cart created successfully with ID: {cartDto.Id}");

                return cartEntity.Id;
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
                var entity = await _unitOfWork.CartRepository.GetCartWithSeatsAsync(cartId).ConfigureAwait(false);
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
                var cart = await _unitOfWork.CartRepository.GetCartWithSeatsAsync(eventSeatDto.CartId).ConfigureAwait(false);
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
