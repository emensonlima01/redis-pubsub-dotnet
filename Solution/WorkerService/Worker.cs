using Domain.Events;
using Domain.Services;

namespace WorkerService
{
    public class Worker(
        IMessageBusService messageBusService,
        IEventHandler eventHandler) : BackgroundService
    {
        private const string PaymentReceivedChannel = "payment:received";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await messageBusService.SubscribeAsync<PaymentReceivedEvent>(
                PaymentReceivedChannel,
                eventHandler
            );

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
