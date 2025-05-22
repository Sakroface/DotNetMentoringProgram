using System;
using System.Collections.Generic;

namespace TicketingSystem.Models
{
    public class CartModel
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
        public int StatusId { get; set; }

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
        public IEnumerable<EventSeatModel> EventSeats { get; set; }

        /// <summary>
        /// Total price for all items in the cart.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
