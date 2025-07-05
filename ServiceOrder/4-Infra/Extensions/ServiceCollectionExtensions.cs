using MongoDB.Driver;
using ServiceOrder.Application.Interfaces;
using ServiceOrder.Application.Services;
using ServiceOrder.Infra.Data;
using ServiceOrder.Infra.Data.Repositories;

namespace ServiceOrder.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
        
        if (mongoDbSettings == null)
            throw new InvalidOperationException("MongoDbSettings não encontrado na configuração");

        var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);

        services.AddSingleton<IMongoDatabase>(mongoDatabase);
        
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();
        
        return services;
    }
} 