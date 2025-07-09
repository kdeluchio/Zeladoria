using RabbitMQ.Client;
using ServiceAuth.Domain.Interfaces;
using ServiceAuth.Domain.Models;
using ServiceAuth.Infra.Data;
using System.Text;
using System.Text.Json;

namespace ServiceAuth.Application.Services;

public class QueueProducerService : IQueueProducerService
{
    private readonly RabbitMQSettings _rabbitMQSettings;

    public QueueProducerService(RabbitMQSettings rabbitMQSettings)
    {
        _rabbitMQSettings = rabbitMQSettings;
    }

    public async Task SendAsync(QueueMessage message)
    {
        var connection = await CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        using (connection)
        {
            using (channel)
            {
                try
                {
                    channel.QueueDeclareAsync(
                        queue: _rabbitMQSettings.QueueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var jsonMessage = JsonSerializer.Serialize(message);
                    var body = Encoding.UTF8.GetBytes(jsonMessage);

                    await channel.BasicPublishAsync(exchange: "",
                                                    routingKey: _rabbitMQSettings.QueueName,
                                                    body: body);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Erro ao enviar mensagem para a fila {_rabbitMQSettings.QueueName}: {ex.Message}", ex);
                }
            }

        }
    }

    private async Task<IConnection> CreateConnectionAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMQSettings.HostName,
            UserName = _rabbitMQSettings.UserName,
            Password = _rabbitMQSettings.Password
        };

        return await factory.CreateConnectionAsync();
    }

}
