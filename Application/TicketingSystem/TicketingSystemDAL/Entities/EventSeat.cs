using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class EventSeat : BaseEntity
    {
        public EventSeat()
        {
            Prices = new HashSet<Price>();
        }

        /// <summary>
        /// Id of the seat it refers to.
        /// </summary>
        public int SeatId { get; set; }

        /// <summary>
        /// Id of the event it belongs to.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Id of the status it has.
        /// </summary>
        [Required]
        public int StatusId { get; set; }

        #region Navigational properties for EF.

        [ForeignKey("SeatId")]
        public virtual VenueSeat Seat { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("StatusId")]
        public virtual EventSeatStatus Status { get; set; }

        public Cart Cart { get; set; }

        public virtual ICollection<Price> Prices { get; set; }

        #endregion
    }
}
