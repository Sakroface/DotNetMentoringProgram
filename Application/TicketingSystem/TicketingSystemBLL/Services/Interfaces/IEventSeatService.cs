using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystemBLL.DTO;

namespace TicketingSystemBLL.Services.Interfaces
{
    /// <summary>
    /// Interface to interact with the event seats.
    /// </summary>
    public interface IEventSeatService
    {
        /// <summary>
        /// Method to retrieve event seat.
        /// </summary>
        /// <param name="seatId">Id of the seat.</param>
        /// <returns>Dto with the details.</returns>
        Task<EventSeatDto> GetEventSeatAsync(int seatId);

        /// <summary>
        /// Method to update seats status.
        /// </summary>
        /// <param name="eventSeatId">Collection of the ids of the seats that will be updated.</param>
        /// <param name="status">Status that will be assigned to them.</param>
        /// <returns>Task after code execution.</returns>
        Task UpdateEventSeatsStatusAsync(IEnumerable<int> eventSeatId, Enums.EventSeatStatus status);
    }
}
