using Microsoft.Extensions.Hosting;

namespace Application.Services
{
    public  class NotificationBackgroundService : BackgroundService
    {
        private readonly NotificationService _notificationService;
        private readonly ILogger<NotificationBackgroundService> _logger;
        public NotificationBackgroundService(NotificationService notificationService, ILogger<NotificationBackgroundService> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
               await  _notificationService.CheckAndSendNotifications();
                _logger.LogInformation("Notification check completed.");

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
