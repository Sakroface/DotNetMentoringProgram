using System.Threading.Tasks;
using TicketingSystemBLL.DTO;

namespace TicketingSystemBLL.Services.Interfaces
{
    public interface INotificationService
    {
        /// <summary>
        /// Method to send notification on the seat status change.
        /// </summary>
        /// <param name="cart">Cart dto with all the information.</param>
        /// <param name="eventSeat">Event sea dto with all the information.</param>
        /// <returns>Task after code execution.</returns>
        Task SendSeatBookingNotificationAsync(CartDto cart, EventSeatDto eventSeat);
    }
}
