using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystemBLL.Objects;

namespace TicketingSystemBLL.Services.Interfaces
{
    /// <summary>
    /// Service to handle event related business logic.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Method to create all events.
        /// </summary>
        /// <param name="obj">Event object that represents the event.</param>
        /// <returns>True - if add operation was completed successfully. False - otherwise.</returns>
        bool CreateEvent(Event obj);

        /// <summary>
        /// Method to get all events.
        /// </summary>
        /// <returns>Events collection.</returns>
        Task<IEnumerable<Event>> GetAllEventsAsync();
    }
}
