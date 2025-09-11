using System.Collections.Generic;

namespace TicketingSystem.Models
{
    public class VenueSectionModel
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Venue Id. 
        /// </summary>
        public int VenueId { get; set; }

        /// <summary>
        /// Name of the section.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Venue rows.
        /// </summary>
        public IEnumerable<VenueRowModel> VenueRows { get; set; }
    }
}
