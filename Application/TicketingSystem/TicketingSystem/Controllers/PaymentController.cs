using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TicketingSystemBLL.Enums;
using TicketingSystemBLL.Services.Interfaces;

namespace TicketingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentController(ILogger<EventController> logger, IMapper mapper, IOrderService orderService, IPaymentService paymentService)
        {
            _logger = logger;
            _mapper = mapper;
            _orderService = orderService;
            _paymentService = paymentService;
        }

        [HttpGet("/payments/{paymentId}")]
        public async Task<ActionResult<int>> GetPaymentStatusAsync(string paymentId)
        {
            try
            {
                if (!Guid.TryParse(paymentId, out Guid Id))
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new { message = "PaymentId should be a proper Guid value with Id." });
                }

                var dto = await _paymentService.GetPaymentAsync(Id);

                if (dto == null)
                {
                    return NotFound();
                }

                return Ok(dto.Status);
            }
            catch (Exception ex)
            {
                _logger.LogError($"PaymentController.GetPaymentStatusAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving payment status." });
            }
        }

        [HttpPost("/payments/{paymentId}/complete")]
        public async Task<ActionResult> CompletePaymentAsync(string paymentId)
        {
            try
            {
                if (!Guid.TryParse(paymentId, out Guid id))
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new { message = "CartId should be a proper Guid value." });
                }

                var paymentDto = await _paymentService.GetPaymentAsync(id);
                var cartDto = await _orderService.GetCartAsync(paymentDto.CartId);

                await _paymentService.UpdatePaymentStatusAsync(id, cartDto, PaymentStatus.Completed);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"PaymentController.CompletePaymentAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while changing payment status." });
            }
        }

        [HttpPost("/payments/{paymentId}/failed")]
        public async Task<ActionResult> FailPaymentAsync(string paymentId)
        {
            try
            {
                if (!Guid.TryParse(paymentId, out Guid id))
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new { message = "CartId should be a proper Guid value." });
                }

                var paymentDto = await _paymentService.GetPaymentAsync(id);
                var cartDto = await _orderService.GetCartAsync(paymentDto.CartId);

                await _paymentService.UpdatePaymentStatusAsync(id, cartDto, PaymentStatus.Failed);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"PaymentController.FailPaymentAsync. Error: {ex.Message}.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while changing payment status." });
            }
        }
    }
}
