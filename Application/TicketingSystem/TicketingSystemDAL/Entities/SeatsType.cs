using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystemDAL.Entities
{
    /// <summary>
    /// Class for the venue type.
    /// </summary>
    public class SeatsType : BaseEntity
    {
        /// <summary>
        /// Contructor by default for EF.
        /// </summary>
        public SeatsType()
        {
            VenueTypes = new HashSet<VenueType>();
            VenueSeats = new HashSet<VenueSeat>();
        }

        /// <summary>
        /// Name for the venue type.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Description for the seats type.
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }


        #region Navigational properties for EF.

        public virtual ICollection<VenueType> VenueTypes { get; set; }

        public virtual ICollection<VenueSeat> VenueSeats { get; set; }

        #endregion
    }
}
