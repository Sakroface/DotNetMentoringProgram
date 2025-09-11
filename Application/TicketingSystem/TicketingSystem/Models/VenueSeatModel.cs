namespace TicketingSystem.Models
{
    public class VenueSeatModel
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
        public int SeatsTypeId { get; set; }
    }
}
