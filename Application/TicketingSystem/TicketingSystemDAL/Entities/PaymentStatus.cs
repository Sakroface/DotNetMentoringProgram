using System.Collections.Generic;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class PaymentStatus : BaseDictionary
    {
        #region Navigation properties for EF

        public virtual ICollection<Payment> Payments { get; set; }

        #endregion
    }
}
