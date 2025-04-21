using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystemDAL.Entities
{
    /// <summary>
    /// Class for the venue type.
    /// </summary>
    public class VenueType : BaseEntity
    {
        /// <summary>
        /// Contructor by default for EF.
        /// </summary>
        public VenueType()
        {
            Venues = new HashSet<Venue>();   
        }

        /// <summary>
        /// Name for the venue type.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Type of the seats for the specified venue type.
        /// </summary>
        [Required]
        public int SeatsTypeId { get; set; }

        // Navigation properties

        #region Navigational properties for EF.
        
        /// <summary>
        /// One to one.
        /// </summary>
        [ForeignKey("SeatsTypeId")]
        public virtual SeatsType SeatsType { get; set; }

        /// <summary>
        /// One to many.
        /// </summary>
        public virtual ICollection<Venue> Venues { get; set; }

        #endregion
    }
}
