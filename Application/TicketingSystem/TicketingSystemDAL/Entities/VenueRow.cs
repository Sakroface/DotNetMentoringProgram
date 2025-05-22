using System.Collections.Generic;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class VenueRow : BaseEntity
    {
        /// <summary>
        /// Contructor by default.
        /// </summary>
        public VenueRow()
        {
            VenueSeats = new HashSet<VenueSeat>();
        }

        /// <summary>
        /// Identifier of the section.
        /// </summary>
        public int SectionId { get; set; }

        /// <summary>
        /// Name of the row.
        /// </summary>
        public string Name { get; set; }

        #region Navigational properties for EF.

        /// <summary>
        /// Relational property for EF(one to one).
        /// </summary>
        public virtual VenueSection Section { get; set; }

        /// <summary>
        /// One to many.
        /// </summary>
        public virtual ICollection<VenueSeat> VenueSeats { get; set; }

        #endregion
    }
}
