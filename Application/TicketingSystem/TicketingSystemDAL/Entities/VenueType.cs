using System.Collections.Generic;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    /// <summary>
    /// Class for the venue type.
    /// </summary>
    public class VenueType : BaseDictionary
    {
        /// <summary>
        /// Contructor by default for EF.
        /// </summary>
        public VenueType()
        {
            Venues = new HashSet<Venue>();   
        }

        #region Navigational properties for EF.

        /// <summary>
        /// One to many.
        /// </summary>
        public virtual ICollection<Venue> Venues { get; set; }

        #endregion
    }
}
