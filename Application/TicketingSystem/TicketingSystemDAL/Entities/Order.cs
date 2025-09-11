using System;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class Order : BaseGuidEntity
    {
        public Guid CartId { get; set; }

        public Guid PaymentId { get; set; }

        public int StatusId { get; set; }

        public Guid UserId { get; set; }

        public DateTime TimeStamp { get; set; }

        #region Naigational properties for EF

        public virtual Cart Cart { get; set; }

        public virtual Payment Payment { get; set; }

        public virtual OrderStatus Status { get; set; }

        public virtual User User { get; set; }

        #endregion
    }
}
