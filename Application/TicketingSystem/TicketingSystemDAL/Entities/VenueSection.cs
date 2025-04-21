using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystemDAL.Entities
{
    public class VenueSection : BaseEntity
    {
        /// <summary>
        /// Constructor by default for the EF.
        /// </summary>
        public VenueSection ()
        {
            VenueRows = new HashSet<VenueRow>();
        }

        /// <summary>
        /// Id of the venue.
        /// </summary>
        [Required]
        public int VenueId { get; set; }

        /// <summary>
        /// Name of the section.
        /// </summary>
        [Required]
        public string Name { get; set; }

        #region Navigational properties for EF.

        /// <summary>
        /// Relational property for EF (one to many).
        /// </summary>
        public virtual Venue Venue { get; set; }

        /// <summary>
        /// One to many.
        /// </summary>
        public virtual ICollection<VenueRow> VenueRows { get; set; }

        #endregion
    }
}
