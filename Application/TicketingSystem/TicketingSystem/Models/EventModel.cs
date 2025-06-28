using System;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.Models
{
    public class EventModel : BaseModel
    {
        /// <summary>
        /// Name of the event.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Date and time of the latest changes to the model.
        /// </summary>
        public DateTime LastModified { get; set; }

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
    }
}
