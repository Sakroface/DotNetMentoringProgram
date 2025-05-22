using System.Collections.Generic;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class Venue : BaseEntity
    {
        /// <summary>
        /// Name of the venue.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the venue. Possible options: StadiumWithSeats, StadiumWithStandOnlyPlaces
        /// </summary>
        public int VenueTypeId { get; set; }

        /// <summary>
        /// NumberOfSeatsAvailable in the venue.
        /// </summary>
        public int NumberOfSeats { get; set; }

        #region Navigational properties for EF

        /// <summary>
        /// Relational entity for EF (many-to-many).
        /// </summary>
        public virtual ICollection<EventVenue> EventVenues { get; set; }

        /// <summary>
        /// Relational entity for EF (one-to-many).
        /// </summary>
        public virtual ICollection<VenueSection> VenueSections { get; set; }

        /// <summary>
        /// One to one.
        /// </summary>
        public virtual VenueType VenueType { get; set; }

        #endregion
    }
}
