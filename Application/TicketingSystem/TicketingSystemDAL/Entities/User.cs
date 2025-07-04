using System.Collections.Generic;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class User : BaseGuidEntity
    {
        /// <summary>
        /// First name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Midddle name of the user.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// UserName for the user.
        /// </summary>
        public string UserName { get; set; } 

        /// <summary>
        /// Password for the user.
        /// </summary>
        public string Password { get; set; }

        public string Email { get; set; }

        #region Navigation properties for EF

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }

        #endregion
    }
}
