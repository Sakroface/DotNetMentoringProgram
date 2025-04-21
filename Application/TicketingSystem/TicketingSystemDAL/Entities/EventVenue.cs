using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystemDAL.Entities
{
    /// <summary>
    /// Mapping of events to venues entity.
    /// </summary>
    public class EventVenue
    {
        /// <summary>
        /// Id of related event.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Id of the related venue.
        /// </summary>
        public int VenueId { get; set; }

        /// <summary>
        /// Relational property for EF(one to one).
        /// </summary>
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        /// <summary>
        /// Relational property for EF(one to one).
        /// </summary>
        [ForeignKey("VenueId")]
        public virtual Venue Venue { get; set; }
    }
}
