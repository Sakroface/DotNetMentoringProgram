using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystemBLL.DTO;

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
        /// <param name="dto">Event object that represents the event.</param>
        /// <returns>Id of the created entity.</returns>
        Task<int> CreateEventAsync(EventDto dto);

        /// <summary>
        /// Method to update event BL.
        /// </summary>
        /// <param name="dto">Dto with the event data.</param>
        void UpdateEvent(EventDto dto);

        /// <summary>
        /// Method to delete the selected event.
        /// </summary>
        /// <param name="eventId">Id of the event to delete.</param>
        void DeleteEvent(int eventId);

        /// <summary>
        /// Method to approve event.
        /// </summary>
        /// <param name="eventId">Id of the event to approve.</param>
        /// <returns>Task from the code execution.</returns>
        Task ApproveEventAsync(int eventId);

        /// <summary>
        /// Method to update status for the event to the cancelled status.
        /// </summary>
        /// <param name="eventId">Id of the event to cancel.</param>
        /// <returns>Task from the code execution.</returns>
        Task CancelEventAsync(int eventId);

        /// <summary>
        /// Method to get all events.
        /// </summary>
        /// <returns>Events collection.</returns>
        Task<IEnumerable<EventDto>> GetAllEventsAsync();

        /// <summary>
        /// Method to update event status.
        /// </summary>
        /// <param name="eventId">Id of the event that we update status for.</param>
        /// <param name="eventStatus">Status of the event that we assign to the event.</param>
        /// <returns>Task from the code execution.</returns>
        Task UpdateEventStatusAsync(int eventId, Enums.EventStatus eventStatus);


        /// <summary>
        /// Method to get the specific event by it's Id.
        /// </summary>
        /// <param name="eventId">Id of the event we are trying to retrieve.</param>
        /// <returns>Event dto.</returns>
        Task<EventDto> GetEventByIdAsync(int eventId);
    }
}
