using System.Collections.Generic;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class EventStatus : BaseDictionary
    {
        /// <summary>
        /// Contructor by default for EF.
        /// </summary>
        public EventStatus()
        {
            Events = new HashSet<Event>();   
        }

        #region Navigation properties for EF.

        public ICollection<Event> Events { get; set; }

        #endregion
    }
}
