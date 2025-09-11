using TicketingSystemBLL.Enums;

namespace TicketingSystemBLL.DTO
{
    public class VenueSeatDto
    {
        /// <summary>
        /// Id of the row it belongs to.
        /// </summary>
        public int RowId { get; set; }

        /// <summary>
        /// Seat number in the row.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Type of the seat.
        /// </summary>
        public SeatType SeatsType { get; set; }
    }
}
