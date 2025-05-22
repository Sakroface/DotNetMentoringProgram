using System;
using System.Threading.Tasks;
using TicketingSystemBLL.DTO;

namespace TicketingSystemBLL.Services.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Method to retrieve cart.
        /// </summary>
        /// <param name="cartId">Unique identifier for the cart.</param>
        /// <returns>Cart model with all information.</returns>
        Task<CartDto> GetCartAsync(Guid cartId);

        /// <summary>
        /// Method to create cart.
        /// </summary>
        /// <param name="cartDto">Cart with all the details.</param>
        /// <returns>Task after code execution.</returns>
        Task CreateCartAsync(CartDto cartDto);

        /// <summary>
        /// Adds seat to the cart.
        /// </summary>
        /// <param name="eventSeatDto">Event seat that will be added to cart.</param>
        /// <returns>Cart with all details.</returns>
        Task<CartDto> AddSeatToCartAsync(EventSeatDto eventSeatDto);

        /// <summary>
        /// Removes the seat from the cart.
        /// </summary>
        /// <param name="eventSeatDto">Dto for the seat to be removed.</param>
        /// <returns>Task after code execution.</returns>
        Task RemoveSeatFromCartAsync(EventSeatDto eventSeatDto);

        /// <summary>
        /// Books all seats in the cart.
        /// </summary>
        /// <param name="cartId">Unique identifier for the cart.</param>
        /// <returns>Payment id.</returns>
        Task<PaymentDto> BookSeatsAsync(Guid cartId);
    }
}
