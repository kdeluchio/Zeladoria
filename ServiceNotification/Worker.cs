using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ServiceNotification;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async override Task StartAsync(CancellationToken cancellationToken)
    {
        var rabbitConfig = _configuration.GetSection("RabbitMQ");
        var factory = new ConnectionFactory()
        {
            HostName = rabbitConfig["HostName"],
            UserName = rabbitConfig["UserName"],
            Password = rabbitConfig["Password"]
        };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.QueueDeclareAsync(queue: rabbitConfig["QueueName"], durable: true, exclusive: false, autoDelete: false, arguments: null);
        await base.StartAsync(cancellationToken);
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var rabbitConfig = _configuration.GetSection("RabbitMQ");
        var queueName = rabbitConfig["QueueName"];
        var consumer = new AsyncEventingBasicConsumer(_channel);
        
        consumer.ReceivedAsync += async(model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"Mensagem recebida da fila: {message}");
                
                // Processa a mensagem aqui
                await ProcessMessageAsync(message);
                
                // Confirma o processamento da mensagem
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
                
                _logger.LogInformation($"Mensagem processada com sucesso: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar mensagem");
                
                // Rejeita a mensagem em caso de erro
                try
                {
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
                catch (Exception nackEx)
                {
                    _logger.LogError(nackEx, "Erro ao rejeitar mensagem");
                }
            }
        };

        // Configura o consumer com autoAck: false para controle manual
        await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
        
        // Mantém o serviço rodando
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task ProcessMessageAsync(string message)
    {
        // Implemente aqui a lógica de processamento da mensagem
        _logger.LogInformation($"Processando mensagem: {message}");
        
        // Simula algum processamento
        await Task.Delay(100);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
            _channel.Dispose();
        }

        if (_connection != null)
        {
            await _connection.CloseAsync();
            _connection.Dispose();
        }

        await Task.CompletedTask;
    }
}
