using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class Price : BaseGuidEntity
    {
        public int EventSeatId { get; set; }

        /// <summary>
        /// FK for the seat type.
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

        public SeatsType SeatType { get; set; }

        [ForeignKey("EventSeatId")]
        public EventSeat EventSeat { get; set; }

        #endregion
    }

}
