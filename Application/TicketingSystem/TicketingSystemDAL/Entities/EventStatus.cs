using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystemDAL.Entities
{
    public class EventStatus : BaseEntity
    {
        /// <summary>
        /// Contructor by default for EF.
        /// </summary>
        public EventStatus()
        {
            Events = new HashSet<Event>();   
        }

        /// <summary>
        /// Name for the event status.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        #region Navigation properties for EF.

        public ICollection<Event> Events { get; set; }

        #endregion
    }
}
