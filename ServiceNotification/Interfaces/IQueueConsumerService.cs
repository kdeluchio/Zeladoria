namespace ServiceNotification.Interfaces;

public interface IQueueConsumerService
{
    Task StartConsumingAsync(CancellationToken cancellationToken);
    Task StopConsumingAsync();
}
