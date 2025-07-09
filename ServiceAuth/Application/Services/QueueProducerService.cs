using ServiceAuth.Domain.Interfaces;
using ServiceAuth.Domain.Models;

namespace ServiceAuth.Application.Services;

public class QueueProducerService : IQueueProducerService
{
    public Task SendAsync(QueueMessage message)
    {
        throw new NotImplementedException();
    }
}
