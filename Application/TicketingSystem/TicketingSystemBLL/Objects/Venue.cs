using TicketingSystemBLL.Enums;

namespace TicketingSystemBLL.Objects
{
    /// <summary>
    /// Venue that can be used by the event.
    /// </summary>
    public class Venue
    {
        /// <summary>
        /// Unique identifier of the venue.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Name of the venue.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Type of the venue. Possible options: StadiumWithSeats, StadiumWithStandOnlyPlaces
        /// </summary>
        VenueType VenueType { get; set; }

        /// <summary>
        /// NumberOfSeatsAvailable in the venue.
        /// </summary>
        int NumberOfSeats { get; set; }
    }
}
