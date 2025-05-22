using System;
using TicketingSystemBLL.Enums;

namespace TicketingSystemBLL.DTO
{
    public class OrderDto
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Id of the related cart.
        /// </summary>
        public Guid CartId { get; set; }

        /// <summary>
        /// Id of the associated payment.
        /// </summary>
        public Guid PaymentId { get; set; }

        /// <summary>
        /// Status of the payment.
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// Id of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Date and time of the last operation.
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
