using System;
using TicketingSystemBLL.Enums;

namespace TicketingSystemBLL.DTO
{
    /// <summary>
    /// Class for the events objects.
    /// </summary>
    public class EventDto
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date and time of the latest changes to the model.
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Date and time of the start of the event.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date and time of the end of the event.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Status of the event.
        /// </summary>
        public EventStatus EventStatus { get; set; }

        /// <summary>
        /// Time(in minutes) required for the setup of the event.
        /// </summary>
        public int SetupTime { get; set; }

        /// <summary>
        /// Time(in minutes) required for the teardown of the event.
        /// </summary>
        public int TeardownTime { get; set; }

        /// <summary>
        /// Event description.
        /// </summary>
        public string Description { get; set; }
    }
}
