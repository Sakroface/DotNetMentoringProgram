using System.Threading.Tasks;
using TicketingSystemBLL.DTO;

namespace TicketingSystemBLL.Services.Interfaces
{
    public interface IMessageQueueService
    {
        Task PublishSeatBookingNotificationAsync(SeatBookingNotification notification);
    }
}
