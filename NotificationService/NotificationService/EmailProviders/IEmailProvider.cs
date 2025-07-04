using NotificationService.Models;
using System.Threading.Tasks;

namespace NotificationService.EmailProviders
{
    public interface IEmailProvider
    {
        Task<EmailResponse> SendEmailAsync(EmailRequest emailRequest);
    }

}
