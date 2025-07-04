using NotificationService.Models;
using System.Threading.Tasks;

namespace NotificationService.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendSeatBookingConfirmationAsync(SeatBookingNotification notification);
    }
}
