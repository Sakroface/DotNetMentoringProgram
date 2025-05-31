using System;
using System.Collections.Generic;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class Order : BaseGuidEntity
    {
        public Order()
        {
            Payments = new HashSet<Payment>();
        }

        public Guid CartId { get; set; }

        public int StatusId { get; set; }

        public Guid UserId { get; set; }

        public DateTime TimeStamp { get; set; }

        #region Naigational properties for EF

        public virtual Cart Cart { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public virtual OrderStatus Status { get; set; }

        public virtual User User { get; set; }

        #endregion
    }
}
