using System.Collections.Generic;
using TicketingSystemBLL.Enums;

namespace TicketingSystemBLL.DTO
{
    /// <summary>
    /// Venue that can be used by the event.
    /// </summary>
    public class VenueDto
    {
        /// <summary>
        /// Unique identifier of the venue.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the venue.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the venue. Possible options: StadiumWithSeats, StadiumWithStandOnlyPlaces
        /// </summary>
        public VenueType VenueType { get; set; }

        /// <summary>
        /// NumberOfSeatsAvailable in the venue.
        /// </summary>
        public int NumberOfSeats { get; set; }

        /// <summary>
        /// Venue sections.
        /// </summary>
        public IEnumerable<VenueSectionDto> VenueSections { get; set; }
    }
}
