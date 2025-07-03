using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class Price : BaseGuidEntity
    {
        /// <summary>
        /// Type of the seat the price was created for.
        /// </summary>
        public int SeatTypeId { get; set; }

        /// <summary>
        /// Price value in the currency.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Flag that indicates whether the current price is active for the seat.
        /// </summary>
        public bool IsActive { get; set; }


        #region Navigational properties for EF

        public virtual ICollection<EventSeat> EventSeats { get; set; }

        [ForeignKey("SeatTypeId")]
        public virtual SeatsType SeatsType { get; set; }

        #endregion
    }

}
