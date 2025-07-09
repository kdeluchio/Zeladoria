using ServiceAuth.Domain.Models;

namespace ServiceAuth.Domain.Interfaces;

public interface IQueueProducerService
{
    Task SendAsync(QueueMessage message);
}
