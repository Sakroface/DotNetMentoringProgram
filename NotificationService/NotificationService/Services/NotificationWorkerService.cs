using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using NotificationService.Services.Interfaces;

namespace NotificationService.Services
{
    public class NotificationWorkerService : BackgroundService
    {
        private readonly ILogger<NotificationWorkerService> _logger;
        private readonly INotificationProcessor _notificationProcessor;
        private readonly IConfiguration _configuration;

        public NotificationWorkerService(
            ILogger<NotificationWorkerService> logger,
            INotificationProcessor notificationProcessor,
            IConfiguration configuration)
        {
            _logger = logger;
            _notificationProcessor = notificationProcessor;
            _configuration = configuration;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ticketing Notification Service starting at: {time}", DateTimeOffset.Now);
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Ticketing Notification Service is running.");

            try
            {
                await _notificationProcessor.StartProcessingAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in notification processing");
                throw;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ticketing Notification Service stopping at: {time}", DateTimeOffset.Now);
            await _notificationProcessor.StopProcessingAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
