using FomularOne.Contracts;
using MassTransit;

namespace FomularOne.MessageQueue
{
    public class DriverNotificationPublishService: IDriverNotificationPublishService
    {
        private readonly ILogger<DriverNotificationPublishService> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public DriverNotificationPublishService(
            ILogger<DriverNotificationPublishService> logger,
            IPublishEndpoint publishEndpoint
            )
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }
        public async Task SendNotification(Guid driverId, string teamName)
        {
            _logger.LogInformation($"Driver Notification for {driverId}");

            await _publishEndpoint.Publish(new NotificationRecord(driverId, teamName));
        }
    }
}
