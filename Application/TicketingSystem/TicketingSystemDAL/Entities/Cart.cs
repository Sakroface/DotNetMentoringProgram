using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    /// <summary>
    /// Entity for the carts.
    /// </summary>
    public class Cart : BaseGuidEntity
    {
        public Cart()
        {
        }

        /// <summary>
        /// Id of the related event.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Id of the cart status.
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Price for the seats.
        /// </summary>
        public Guid PriceId { get; set; } 

        /// <summary>
        /// Id of the user that cart belongs to.
        /// </summary>
        public Guid UserId { get; set; }

        #region Navigation properties for EF

        /// <summary>
        /// Event that order belongs to.
        /// </summary>
        public virtual Event Event { get; set; }

        /// <summary>
        /// One to one.
        /// </summary>
        [ForeignKey("StatusId")]
        public virtual CartStatus Status { get; set; }

        /// <summary>
        /// One to one.
        /// </summary>
        public virtual Payment Payment { get; set; }

        /// <summary>
        /// Price for the Cart.
        /// </summary>
        public virtual Price Price { get; set; }

        /// <summary>
        /// Seats added to the cart.
        /// </summary>
        public virtual ICollection<EventSeat> EventSeats { get; set; }

        /// <summary>
        /// User that order belongs to.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Order that cart belongs to.
        /// </summary>
        public virtual Order Order { get; set; }

        #endregion

    }
}
