using ServiceNotification.Interfaces;
using ServiceNotification.Services;

namespace ServiceNotification;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IQueueConsumerService _queueConsumerService;

    public Worker(ILogger<Worker> logger, IQueueConsumerService queueConsumerService)
    {
        _logger = logger;
        _queueConsumerService = queueConsumerService;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Iniciando serviço de notificação...");
            
            await _queueConsumerService.StartConsumingAsync(stoppingToken);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante a execução do serviço de notificação");
        }
        finally
        {
            await _queueConsumerService.StopConsumingAsync();
            _logger.LogInformation("Serviço de notificação finalizado");
        }
    }
}
