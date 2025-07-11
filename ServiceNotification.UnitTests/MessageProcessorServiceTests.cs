using Microsoft.Extensions.Logging;
using Moq;
using ServiceNotification.Models;
using ServiceNotification.Services;
using System.Text.Json;

namespace ServiceNotification.UnitTests;

public class MessageProcessorServiceTests
{
    [Fact]
    public async Task Should_ValidateMessage_And_Process_LogingInformation()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<MessageProcessorService>>();
        var service = new MessageProcessorService(loggerMock.Object);
        var messageData = new MessageData { Email = "teste@teste.com" };
        var message = JsonSerializer.Serialize(messageData);

        // Act
        await service.ProcessMessageAsync(message);

        // Assert
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Evento lido da fila e enviado")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
} 