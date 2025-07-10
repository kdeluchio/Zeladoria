using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceNotification.Interfaces;
using System.Text;

namespace ServiceNotification.Services;

public class QueueConsumerService : IQueueConsumerService, IDisposable
{
    private readonly ILogger<QueueConsumerService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMessageProcessorService _messageProcessorService;
    private IConnection? _connection;
    private IChannel? _channel;
    private AsyncEventingBasicConsumer? _consumer;
    private string? _queueName;
    private bool _isConsuming = false;

    public QueueConsumerService(
        ILogger<QueueConsumerService> logger, 
        IConfiguration configuration,
        IMessageProcessorService messageProcessorService)
    {
        _logger = logger;
        _configuration = configuration;
        _messageProcessorService = messageProcessorService;
    }

    public async Task StartConsumingAsync(CancellationToken cancellationToken)
    {
        try
        {
            await InitializeConnectionWithRetryAsync(cancellationToken);
            await SetupConsumerAsync();
            _isConsuming = true;
            _logger.LogInformation("Serviço de consumo da fila iniciado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao iniciar o serviço de consumo da fila");
            throw;
        }
    }

    public async Task StopConsumingAsync()
    {
        try
        {
            _isConsuming = false;
            
            if (_consumer != null)
            {
                _consumer.ReceivedAsync -= OnMessageReceived;
            }

            if (_channel != null)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
                _channel = null;
            }

            if (_connection != null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
                _connection = null;
            }

            _logger.LogInformation("Serviço de consumo da fila parado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao parar o serviço de consumo da fila");
        }
    }

    private async Task InitializeConnectionWithRetryAsync(CancellationToken cancellationToken)
    {
        var rabbitConfig = _configuration.GetSection("RabbitMQ");
        var factory = new ConnectionFactory()
        {
            HostName = rabbitConfig["HostName"],
            UserName = rabbitConfig["UserName"],
            Password = rabbitConfig["Password"]
        };

        var maxRetries = 30; // Máximo de 30 tentativas
        var baseDelay = 2000; // 2 segundos base
        
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                _logger.LogInformation($"Tentativa {attempt} de conectar ao RabbitMQ...");
                
                _connection = await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
                _queueName = rabbitConfig["QueueName"];

                await _channel.QueueDeclareAsync(
                    queue: _queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                _logger.LogInformation($"Conexão com RabbitMQ estabelecida na tentativa {attempt}. Fila: {_queueName}");
                return; // Sucesso, sai do loop
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Tentativa {attempt} falhou: {ex.Message}");
                
                if (attempt == maxRetries)
                {
                    _logger.LogError($"Falha ao conectar ao RabbitMQ após {maxRetries} tentativas");
                    throw;
                }

                // Delay exponencial: 2s, 4s, 8s, 16s, 32s, etc. (máximo 60s)
                var delay = Math.Min(baseDelay * Math.Pow(2, attempt - 1), 60000);
                _logger.LogInformation($"Aguardando {delay}ms antes da próxima tentativa...");
                
                await Task.Delay((int)delay, cancellationToken);
            }
        }
    }

    private async Task SetupConsumerAsync()
    {
        if (_channel == null || _queueName == null)
        {
            throw new InvalidOperationException("Conexão com RabbitMQ não foi inicializada");
        }

        _consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer.ReceivedAsync += OnMessageReceived;

        await _channel.BasicConsumeAsync(
            queue: _queueName,
            autoAck: false,
            consumer: _consumer);

        _logger.LogInformation("Consumer configurado e iniciado");
    }

    private async Task OnMessageReceived(object model, BasicDeliverEventArgs ea)
    {
        try
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            _logger.LogInformation($"Mensagem recebida da fila: {message}");
            
            await _messageProcessorService.ProcessMessageAsync(message);
            
            if (_channel != null)
            {
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            
            _logger.LogInformation($"Mensagem processada com sucesso: {message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar mensagem");
            
            // Rejeita a mensagem em caso de erro
            try
            {
                if (_channel != null)
                {
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            }
            catch (Exception nackEx)
            {
                _logger.LogError(nackEx, "Erro ao rejeitar mensagem");
            }
        }
    }

    public void Dispose()
    {
        StopConsumingAsync().Wait();
    }
} 