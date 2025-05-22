using System.Collections.Generic;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class EventSeatStatus : BaseDictionary
    {
        /// <summary>
        /// Constructor by default.
        /// </summary>
        public EventSeatStatus()
        {
            EventSeats = new HashSet<EventSeat>();
        }

        #region Navigational properties for EF

        public virtual ICollection<EventSeat> EventSeats { get; set; }

        #endregion
    }
}
