using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.Models
{
    public class OrderModel
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
        public int StatusId { get; set; }

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
