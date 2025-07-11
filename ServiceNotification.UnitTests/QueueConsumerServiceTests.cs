using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceNotification.Interfaces;
using ServiceNotification.Services;

namespace ServiceNotification.UnitTests;

public class QueueConsumerServiceTests
{
    [Fact]
    public async Task Should_ThrowsException_When_Config_Has_Error()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<QueueConsumerService>>();
        var configMock = new Mock<IConfiguration>();
        var processorMock = new Mock<IMessageProcessorService>();
        var service = new QueueConsumerService(loggerMock.Object, configMock.Object, processorMock.Object);
        var token = new CancellationTokenSource().Token;

        // Forçar erro de configuração
        configMock.Setup(x => x.GetSection(It.IsAny<string>())).Throws(new System.Exception("Config error"));

        // Act & Assert
        await Assert.ThrowsAsync<System.Exception>(() => service.StartConsumingAsync(token));
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Erro ao iniciar o serviço de consumo da fila")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

} 