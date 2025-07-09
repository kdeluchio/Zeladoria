using ServiceNotification;
using ServiceNotification.Interfaces;
using ServiceNotification.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IMessageProcessorService, MessageProcessorService>();
builder.Services.AddSingleton<IQueueConsumerService, QueueConsumerService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
