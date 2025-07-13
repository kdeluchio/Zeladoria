using ServiceAuth.Infra.Extensions;
using ServiceAuth.Presentation;

namespace ServiceAuth;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddMongoDb(builder.Configuration);
        builder.Services.AddRabbitMQ(builder.Configuration);
        builder.Services.AddApplicationServices();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapAuthEndpoints();

        app.Run();
    }
}
