using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystemDAL.Entities
{
    public class VenueSeat : BaseEntity
    {
        /// <summary>
        /// Contructor by default.
        /// </summary>
        public VenueSeat()
        {
            EventSeats = new HashSet<EventSeat>();
        }

        /// <summary>
        /// Id of the row it belongs to.
        /// </summary>
        [Required]
        public int RowId { get; set; }

        /// <summary>
        /// Seat number in the row.
        /// </summary>
        [Required]
        public int Number { get; set; }

        /// <summary>
        /// Type of the seat.
        /// </summary>
        public int SeatsTypeId { get; set; }

        #region Navigational properties for EF.

        /// <summary>
        /// Many to one.
        /// </summary>
        [ForeignKey("RowId")]
        public virtual VenueRow Row { get; set; }

        /// <summary>
        /// Many to one.
        /// </summary>
        [ForeignKey("SeatsTypeId")]
        public virtual SeatsType SeatsType { get; set; }

        /// <summary>
        /// One to many.
        /// </summary>
        public virtual ICollection<EventSeat> EventSeats { get; set; }

        #endregion
    }
}
