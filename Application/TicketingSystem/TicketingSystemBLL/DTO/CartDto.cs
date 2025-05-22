using System;
using System.Collections.Generic;
using TicketingSystemBLL.Enums;

namespace TicketingSystemBLL.DTO
{
    public class CartDto
    {
        /// <summary>
        /// Id of the cart.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Id of the related event.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Id of the cart status.
        /// </summary>
        public CartStatus Status { get; set; }

        /// <summary>
        /// Price for the seats.
        /// </summary>
        public Guid PriceId { get; set; }

        /// <summary>
        /// Id of the user that cart belongs to.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Event seats that belong to the cart.
        /// </summary>
        public IEnumerable<EventSeatDto> EventSeats { get; set; }

        /// <summary>
        /// Total price for all items in the cart.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
