using System;

namespace TicketingSystem.Models
{
    public class PaymentModel
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Id of the cart that payment belongs to.
        /// </summary>
        public Guid CartId { get; set; }

        /// <summary>
        /// Id of the order that payment belongs to.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Date and time when payment was performed.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Id of the payment status.
        /// </summary>
        public int StatusId { get; set; }
    }
}
