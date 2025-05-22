using System;

namespace TicketingSystem.Models
{
    public class PriceModel
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// FK for the price.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// FK for the seat type.
        /// </summary>
        public int SeatTypeId { get; set; }

        /// <summary>
        /// Price value in the currency.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
