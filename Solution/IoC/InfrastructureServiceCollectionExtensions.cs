using Domain.Repositories;
using Domain.Services;
using Infrastructure.Configuration;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace IoC;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddRedisConfiguration(configuration)
            .AddRedisCache()
            .AddRepositories()
            .AddDomainServices();

        return services;
    }

    private static IServiceCollection AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisSettings>(configuration.GetSection(RedisSettings.SectionName));
        
        return services;
    }

    private static IServiceCollection AddRedisCache(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var redisSettings = configuration.GetSection(RedisSettings.SectionName).Get<RedisSettings>()
                ?? throw new InvalidOperationException("Redis settings not found");

            if (string.IsNullOrWhiteSpace(redisSettings.ConnectionString))
                throw new InvalidOperationException("Redis connection string is required");

            return ConnectionMultiplexer.Connect(redisSettings.ConnectionString);
        });

        services.AddSingleton<ICacheService, RedisCacheService>();
        services.AddSingleton<IMessageBusService, RedisMessageBusService>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IPaymentRepository, PaymentRepository>();

        return services;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services;
    }
}
