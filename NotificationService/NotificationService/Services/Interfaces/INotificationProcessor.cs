using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.Services.Interfaces
{
    public interface INotificationProcessor
    {
        Task StartProcessingAsync(CancellationToken cancellationToken);
        Task StopProcessingAsync();
    }
}
