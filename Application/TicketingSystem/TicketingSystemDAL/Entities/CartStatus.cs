using System.Collections.Generic;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    /// <summary>
    /// Class for the cart status entities.
    /// </summary>
    public class CartStatus : BaseDictionary 
    {
        public CartStatus()
        {
            Carts = new HashSet<Cart>();
        }

        #region Navigational properties for EF

        public virtual ICollection<Cart> Carts { get; set; }

        #endregion
    }
}
