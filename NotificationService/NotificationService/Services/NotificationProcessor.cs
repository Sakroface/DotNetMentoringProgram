using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotificationService.Models;
using NotificationService.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.Services
{
    public class NotificationProcessor : INotificationProcessor, IAsyncDisposable
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private readonly ILogger<NotificationProcessor> _logger;
        private readonly IEmailService _emailService;
        private const string QUEUE_NAME = "seat-booking-notifications";

        public NotificationProcessor(
            ILogger<NotificationProcessor> logger,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _logger = logger;
            _emailService = emailService;

            var connectionString = configuration.GetConnectionString("ServiceBus");
            _client = new ServiceBusClient(connectionString);

            _processor = _client.CreateProcessor(QUEUE_NAME, new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
                ReceiveMode = ServiceBusReceiveMode.PeekLock
            });

            _processor.ProcessMessageAsync += ProcessMessageAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;
        }

        public async Task StartProcessingAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting message processing...");
            await _processor.StartProcessingAsync(cancellationToken);

            // Keep processing until cancellation is requested
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }

        public async Task StopProcessingAsync()
        {
            _logger.LogInformation("Stopping message processing...");
            await _processor.StopProcessingAsync();
        }

        private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            try
            {
                var messageBody = args.Message.Body.ToString();
                _logger.LogInformation($"Processing message: {args.Message.MessageId}");

                var notification = JsonConvert.DeserializeObject<SeatBookingNotification>(messageBody);

                await _emailService.SendSeatBookingConfirmationAsync(notification);

                await args.CompleteMessageAsync(args.Message);

                _logger.LogInformation($"Successfully processed notification with tracking ID: {notification.NotificationTrackingId}");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, $"Failed to deserialize message: {args.Message.MessageId}");
                await args.DeadLetterMessageAsync(args.Message, "DeserializationError", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing message: {args.Message.MessageId}");

                // Abandon the message (will be retried)
                await args.AbandonMessageAsync(args.Message);
            }
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, $"Service Bus processing error. Source: {args.ErrorSource}");
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_processor != null)
                await _processor.DisposeAsync();
            if (_client != null)
                await _client.DisposeAsync();
        }
    }
}
