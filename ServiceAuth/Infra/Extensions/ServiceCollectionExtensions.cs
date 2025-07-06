using MongoDB.Driver;
using ServiceAuth.Application.Services;
using ServiceAuth.Application.Validators;
using ServiceAuth.Infra.Data;
using ServiceAuth.Infra.Data.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using ServiceAuth.Domain.Interfaces;

namespace ServiceAuth.Infra.Extensions;

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
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<LoginModelValidator>();
        services.AddValidatorsFromAssemblyContaining<SignupModelValidator>();
        services.AddValidatorsFromAssemblyContaining<ResetPasswordModelValidator>();
        
        return services;
    }
} 