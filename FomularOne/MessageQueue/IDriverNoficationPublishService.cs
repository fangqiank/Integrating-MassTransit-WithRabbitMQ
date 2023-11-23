namespace FomularOne.MessageQueue
{
    public interface IDriverNotificationPublishService
    {
        Task SendNotification(Guid driverId,  string teamName);
    }
}
