using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TicketingSystemBLL.DTO;
using TicketingSystemBLL.Services.Interfaces;

namespace TicketingSystemBLL.Services
{
    public class AzureServiceBusService : IMessageQueueService, IAsyncDisposable
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        private readonly ILogger<AzureServiceBusService> _logger;
        private const string QUEUE_NAME = "seat-booking-notifications";

        public AzureServiceBusService(ILogger<AzureServiceBusService> logger, IConfiguration configuration)
        {
            _logger = logger;
            var connectionString = configuration.GetConnectionString("ServiceBus");

            _client = new ServiceBusClient(connectionString);
            _sender = _client.CreateSender(QUEUE_NAME);
        }

        public async Task PublishSeatBookingNotificationAsync(string messageBody)
        {
            try
            {
                var messageBody = JsonConvert.SerializeObject(notification);
                var message = new ServiceBusMessage(messageBody)
                {
                    ContentType = "application/json",
                    MessageId = Guid.NewGuid().ToString(),
                    Subject = "SeatBookingNotification"
                };

                await _sender.SendMessageAsync(message);
                _logger.LogInformation($"Seat booking notification sent to Service Bus for user {notification.Parameters.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to Service Bus");
                throw;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_sender != null)
                await _sender.DisposeAsync();
            if (_client != null)
                await _client.DisposeAsync();
        }
    }

}
