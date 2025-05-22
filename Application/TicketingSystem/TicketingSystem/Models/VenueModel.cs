using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.Models
{
    public class VenueModel
    {
        /// <summary>
        /// Unique identifier of the venue.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the venue.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Type of the venue. Possible options: StadiumWithSeats, StadiumWithStandOnlyPlaces
        /// </summary>
        [Required]
        public int TypeId { get; set; }

        /// <summary>
        /// NumberOfSeatsAvailable in the venue.
        /// </summary>
        public int NumberOfSeats { get; set; }

        /// <summary>
        /// Venue sections.
        /// </summary>
        public IEnumerable<VenueSectionModel> VenueSections { get; set; }
    }
}
