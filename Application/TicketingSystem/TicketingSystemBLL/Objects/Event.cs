using System;
using TicketingSystemBLL.Enums;

namespace TicketingSystemBLL.Objects
{
    /// <summary>
    /// Class for the events objects.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Name of the event.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Date and time of the start of the event.
        /// </summary>
        DateTime StartDate { get; set; }

        /// <summary>
        /// Date and time of the end of the event.
        /// </summary>
        DateTime EndDate { get; set; }

        /// <summary>
        /// Status of the event.
        /// </summary>
        EventStatus EventStatus { get; set; }

        /// <summary>
        /// Time(in minutes) required for the setup of the event.
        /// </summary>
        int SetupTime { get; set; }

        /// <summary>
        /// Time(in minutes) required for the teardown of the event.
        /// </summary>
        int TeardownTime { get; set; }

        /// <summary>
        /// Event description.
        /// </summary>
        string Description { get; set; }
    }
}
