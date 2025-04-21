using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystemDAL.Entities
{
    public class EventSeat
    {
        /// <summary>
        /// Id of the seat it refers to.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        public int SeatId { get; set; }

        /// <summary>
        /// Id of the event it belongs to.
        /// </summary>
        [Key]
        [Column(Order = 1)]
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

        #endregion
    }
}
