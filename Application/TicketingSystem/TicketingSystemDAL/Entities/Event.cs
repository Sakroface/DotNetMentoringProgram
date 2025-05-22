using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystemDAL.Entities.Base;

namespace TicketingSystemDAL.Entities
{
    public class Event : BaseEntity
    {
        /// <summary>
        /// Constructor by default for EF.
        /// </summary>
        public Event()
        {
            EventVenues = new HashSet<EventVenue>();
            EventSeats = new HashSet<EventSeat>();
        }

        /// <summary>
        /// Name of the event.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Date and time of the start of the event.
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date and time of the end of the event.
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Status of the event.
        /// </summary>
        [Required]
        public int StatusId { get; set; }

        /// <summary>
        /// Time(in minutes) required for the setup of the event.
        /// </summary>
        [Required]
        public int SetupTime { get; set; }

        /// <summary>
        /// Time(in minutes) required for the teardown of the event.
        /// </summary>
        [Required]
        public int TeardownTime { get; set; }

        /// <summary>
        /// Event description.
        /// </summary>
        public string Description { get; set; }

        #region Navigation properties for EF

        /// <summary>
        /// Relational entity for EF (many-to-many).
        /// </summary>
        public virtual ICollection<EventVenue> EventVenues { get; set; }

        /// <summary>
        /// One to many.
        /// </summary>
        public virtual ICollection<EventSeat> EventSeats { get; set; }

        /// <summary>
        /// One to one.
        /// </summary>
        [ForeignKey("StatusId")]
        public virtual EventStatus Status { get; set; }

        #endregion
    }
}
