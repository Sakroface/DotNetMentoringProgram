using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TicketingSystem.Models;
using TicketingSystemBLL.DTO;
using TicketingSystemBLL.Services.Interfaces;

namespace TicketingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly string ALL_EVENTS_CACHE_KEY;
        private readonly string EVENT_CACHE_KEY_PREFIX;
        private readonly TimeSpan _cacheExpiration;


        public OrderController(ILogger<EventController> logger, IMapper mapper, IOrderService orderService, IMemoryCache cache, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _orderService = orderService;
            _cache = cache;
            _configuration = configuration;

            ALL_EVENTS_CACHE_KEY = _configuration.GetValue("ALL_EVENTS_CACHE_KEY", string.Empty);
            EVENT_CACHE_KEY_PREFIX = _configuration.GetValue("EVENT_CACHE_KEY_PREFIX", string.Empty);
            _cacheExpiration = TimeSpan.FromMinutes(30);
        }

        [HttpGet("/carts/{cartId}")]
        public async Task<ActionResult<CartModel>> GetCartAsync(string cartId)
        {
            try
            {
                if (!Guid.TryParse(cartId, out Guid Id))
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new { message = "CartId should be a proper Guid value with Id." });
                }

                var cartModel = await _orderService.GetCartAsync(Id);

                if (cartModel == null)
                {
                    return NotFound();
                }

                return Ok(cartModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"OrderController.GetCartAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving event." });
            }
        }

        [HttpPost("/orders/carts/{cartId}")]
        public async Task<ActionResult<CartModel>> AddToCartAsync(string cartId, [FromBody] CartItemModel cartItem)
        {
            try
            {
                if (!Guid.TryParse(cartId, out Guid id))
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new { message = "CartId should be a proper Guid value." });
                }


                if (cartItem == null || cartItem.EventId <= 0 || cartItem.SeatId <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new { message = "Request must include event_id, seat_id, and price_id." });
                }

                if (!Guid.TryParse(cartItem.PriceId, out Guid priceId))
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new { message = "priceId should be a proper Guid value." });
                }

                var model = new EventSeatModel(cartItem.SeatId, cartItem.EventId, 1, priceId, id);

                var dto = _mapper.Map<EventSeatDto>(model);

                var updatedCart = await _orderService.AddSeatToCartAsync(dto);

                if (updatedCart == null)
                {
                    return NotFound();
                }

                return Ok(updatedCart);
            }
            catch (Exception ex)
            {
                _logger.LogError($"OrderController.AddToCartAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while adding item to cart." });
            }
        }

        [HttpDelete("/orders/carts/{cartId}/events/{eventId}/seats/{seatId}")]
        public async Task<ActionResult> RemoveSeatFromCartAsync(string cartId, int eventId, int seatId)
        {
            try
            {
                if (!Guid.TryParse(cartId, out Guid id))
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new { message = "CartId should be a proper Guid value." });
                }

                var model = new EventSeatModel(seatId, eventId, id);
                var dto = _mapper.Map<EventSeatDto>(model);

                await _orderService.RemoveSeatFromCartAsync(dto).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"OrderController.RemoveSeatFromCartAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while removing item from cart." });
            }
        }

        [HttpPut("/orders/carts/{cartId}/book")]
        public async Task<ActionResult<PaymentModel>> BookCartAsync(string cartId)
        {
            try
            {
                if (!Guid.TryParse(cartId, out Guid id))
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new { message = "CartId should be a proper Guid value." });
                }

                var paymentModel = await _orderService.BookSeatsAsync(id);

                if (paymentModel == null)
                {
                    return NotFound();
                }

                return Ok(paymentModel.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"OrderController.BookCartAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while booking the cart." });
            }
        }

    }
}
