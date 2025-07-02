using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class EventSeat : BaseEntity
    {
        public EventSeat()
        {
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

        /// <summary>
        /// Date and time when the record has been created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date and time when the record has been booked.
        /// </summary>
        public DateTime BookedAt { get; set; }

        /// <summary>
        /// Id of the price assigned to the seat.
        /// </summary>
        public Guid? PriceId { get; set; }

        /// <summary>
        /// Id of the cart that seat belongs to.
        /// </summary>
        public Guid? CartId { get; set; }

        #region Navigational properties for EF.

        [ForeignKey("SeatId")]
        public virtual VenueSeat Seat { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("StatusId")]
        public virtual EventSeatStatus Status { get; set; }

        [ForeignKey("PriceId")]
        public virtual Price Price { get; set; }

        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }

        #endregion
    }
}
