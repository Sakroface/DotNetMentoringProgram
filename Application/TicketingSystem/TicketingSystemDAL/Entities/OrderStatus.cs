using System.Collections.Generic;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class OrderStatus : BaseDictionary
    {
        #region Navigation properties for EF

        public virtual ICollection<Order> Orders { get; set; }

        #endregion

    }
}
