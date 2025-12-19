using Domain.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services;

public class RedisMessageBusService : IMessageBusService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly JsonSerializerOptions _jsonOptions;

    public RedisMessageBusService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task PublishAsync<T>(string channel, T message) where T : class
    {
        var subscriber = _connectionMultiplexer.GetSubscriber();
        var serializedMessage = JsonSerializer.Serialize(message, _jsonOptions);
        await subscriber.PublishAsync(RedisChannel.Literal(channel), serializedMessage);
    }

    public async Task SubscribeAsync<T>(string channel, IEventHandler eventHandler) where T : class
    {
        var subscriber = _connectionMultiplexer.GetSubscriber();
        
        await subscriber.SubscribeAsync(RedisChannel.Literal(channel), async (_, message) =>
        {
            if (message.HasValue)
            {
                var deserializedMessage = JsonSerializer.Deserialize<T>(message.ToString(), _jsonOptions);
                if (deserializedMessage != null)
                {
                    await eventHandler.Handle(deserializedMessage);
                }
            }
        });
    }
}
