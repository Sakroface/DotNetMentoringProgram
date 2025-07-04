using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NotificationService.Models;
using NotificationService.Services.Interfaces;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationService.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;

        public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendSeatBookingConfirmationAsync(SeatBookingNotification notification)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                using (var client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
                {
                    client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                    client.EnableSsl = bool.Parse(smtpSettings["EnableSsl"] ?? "true");

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpSettings["FromEmail"], smtpSettings["FromName"]),
                        Subject = GetEmailSubject(notification.ActionName),
                        Body = GenerateEmailContent(notification),
                        IsBodyHtml = false
                    };

                    mailMessage.To.Add(notification.Parameters.CustomerEmail);

                    await client.SendMailAsync(mailMessage);
                }

                _logger.LogInformation($"Email sent successfully to {notification.Parameters.CustomerEmail}. Tracking ID: {notification.NotificationTrackingId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {notification.Parameters.CustomerEmail}. Tracking ID: {notification.NotificationTrackingId}");
                throw;
            }
        }

        private string GetEmailSubject(string operationName)
        {
            switch (operationName) 
            {
                case "ticket_added_to_checkout":
                    return "Ticket Added to Cart - Complete Your Purchase";
                case
                "ticket_successfully_checked_out":
                    return "Ticket Purchase Confirmed";
                default: return "Ticketing Notification";
            };
        }

        private string GenerateEmailContent(SeatBookingNotification notification)
        {
            return $@"
                Dear {notification.Parameters.CustomerName},

                {notification.Content.OrderSummary}

                Event: {notification.Content.EventName}
                Date: {notification.Content.EventDate:MMM dd, yyyy}
                Venue: {notification.Content.VenueName}

                Total Amount: ${notification.Content.OrderAmount:F2}

                Tracking ID: {notification.NotificationTrackingId}

                Thank you for choosing our ticketing service!

                Best regards,
                Ticketing Team
            ";
        }
    }
}
