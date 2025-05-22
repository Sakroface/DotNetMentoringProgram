using System;
using TicketingSystemBLL.Enums;

namespace TicketingSystemBLL.DTO
{
    public class PaymentDto
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Amount of money paid.
        /// </summary>
        public decimal Amount { get; set; }

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
        public PaymentStatus Status { get; set; }
    }
}
