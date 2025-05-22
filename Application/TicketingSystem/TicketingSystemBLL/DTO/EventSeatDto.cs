using System;
using TicketingSystemBLL.Enums;

namespace TicketingSystemBLL.DTO
{
    public class EventSeatDto
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of the cart that event Seat belongs to.
        /// </summary>
        public Guid CartId { get; set; }

        /// <summary>
        /// Id of the seat it refers to.
        /// </summary>
        public int SeatId { get; set; }

        /// <summary>
        /// Id of the event it belongs to.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Id of the status it has.
        /// </summary>
        public EventSeatStatus Status { get; set; }

        /// <summary>
        /// Price id for the current cart seat.
        /// </summary>
        public Guid PriceId { get; set; }

        /// <summary>
        /// Property to store price from the price entity.
        /// </summary>
        public decimal Price { get; set; }
    }
}
