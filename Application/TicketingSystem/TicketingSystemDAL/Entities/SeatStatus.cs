using System.Collections.Generic;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class SeatStatus : BaseDictionary
    {
        /// <summary>
        /// Constructor by default.
        /// </summary>
        public SeatStatus()
        {
            VenueSeats = new HashSet<VenueSeat>();
        }

        #region Navigational properties for EF

        public virtual ICollection<VenueSeat> VenueSeats { get; set; }

        #endregion
    }
}
