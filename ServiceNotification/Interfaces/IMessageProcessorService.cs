namespace ServiceNotification.Interfaces;

public interface IMessageProcessorService
{
    Task ProcessMessageAsync(string message);
}
