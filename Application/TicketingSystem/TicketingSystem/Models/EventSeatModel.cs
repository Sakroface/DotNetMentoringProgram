using System;

namespace TicketingSystem.Models
{
    /// <summary>
    /// Model for event seat.
    /// </summary>
    public class EventSeatModel
    {
        /// <summary>
        /// Constructor to init object with params.
        /// </summary>
        /// <param name="seatId">seat Id.</param>
        /// <param name="eventId">Event Id.</param>
        /// <param name="statusId">Status Id.</param>
        /// <param name="priceId">Price Id.</param>
        /// <param name="cartId">Cart Id.</param>
        public EventSeatModel(int seatId, int eventId, int statusId, Guid priceId, Guid cartId)
        {
            SeatId = seatId;
            EventId = eventId;
            StatusId = statusId;
            PriceId = priceId;
            CartId = cartId;
        }

        /// <summary>
        /// Constructor to init object with params.
        /// </summary>
        /// <param name="seatId">seat Id.</param>
        /// <param name="eventId">Event Id.</param>
        /// <param name="cartId">Cart Id.</param>
        public EventSeatModel(int seatId, int eventId, Guid cartId)
        {
            SeatId = seatId;
            EventId = eventId;
            CartId = cartId;
        }

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
        public int StatusId { get; set; }

        /// <summary>
        /// Price id for the current cart seat.
        /// </summary>
        public Guid PriceId { get; set; }
    }
}
