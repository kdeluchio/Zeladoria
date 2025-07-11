using ServiceNotification.Interfaces;
using ServiceNotification.Models;
using System.Text.Json;

namespace ServiceNotification.Services;

public class MessageProcessorService : IMessageProcessorService
{
    private readonly ILogger<MessageProcessorService> _logger;

    public MessageProcessorService(ILogger<MessageProcessorService> logger)
    {
        _logger = logger;
    }

    public async Task ProcessMessageAsync(string message)
    {
        try
        {
            var messageData = ParseMessage(message);
            _logger.LogInformation($"Evento lido da fila e enviado: {message}");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar mensagem");
            throw;
        }
    }

    private MessageData ParseMessage(string message)
    {
        try
        {
            return JsonSerializer.Deserialize<MessageData>(message);
        }
        catch (JsonException)
        {
            return null;
        }
    }

}
