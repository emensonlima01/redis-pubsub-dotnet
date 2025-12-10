namespace Domain.Services;

public interface IMessageBusService
{
    Task PublishAsync<T>(string channel, T message) where T : class;
    Task SubscribeAsync<T>(string channel, Func<T, Task> handler) where T : class;
}
