using System.Collections.Generic;

namespace TicketingSystemBLL.DTO
{
    public class VenueRowDto
    {
        /// <summary>
        /// Unique Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the section.
        /// </summary>
        public int SectionId { get; set; }

        /// <summary>
        /// Name of the row.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Venue seats.
        /// </summary>
        public IEnumerable<VenueSeatDto> VenueSeats { get; set; }
    }
}
