using MongoDB.Driver;
using ServiceOrder.Application.Services;
using ServiceOrder.Application.Validators;
using ServiceOrder.Infra.Data;
using ServiceOrder.Infra.Data.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using ServiceOrder.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateOrderModelValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateServiceModelValidator>();
        
        // Registrar HttpContextAccessor para acessar ClaimsPrincipal
        services.AddHttpContextAccessor();
        
        // Registrar UserContextService para facilitar acesso aos dados do usuário
        services.AddScoped<IUserContextService, UserContextService>();
        
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        if (string.IsNullOrEmpty(secretKey))
            throw new InvalidOperationException("JwtSettings:SecretKey não encontrado na configuração");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();

        return services;
    }
} 