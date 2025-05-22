using System;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class Payment : BaseGuidEntity
    {
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
        public int StatusId  { get;set; }

        #region Navigational properties for EF

        public virtual Order Order { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual PaymentStatus Status { get; set; }

        #endregion
    }
}
