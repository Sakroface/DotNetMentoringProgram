using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystemDAL.Entities
{
    public class EventSeatStatus : BaseEntity
    {
        /// <summary>
        /// Constructor by default.
        /// </summary>
        public EventSeatStatus()
        {
            EventSeats = new HashSet<EventSeat>();
        }

        /// <summary>
        /// Name of the seat status.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        #region Navigational properties for EF

        public virtual ICollection<EventSeat> EventSeats { get; set; }

        #endregion
    }
}
