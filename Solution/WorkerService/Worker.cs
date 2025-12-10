using Application.UseCases;
using Domain.Events;
using Domain.Services;

namespace WorkerService
{
    public class Worker(
        IMessageBusService messageBusService,
        ProcessPaymentUseCase processPaymentUseCase) : BackgroundService
    {
        private const string PaymentReceivedChannel = "payment:received";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await messageBusService.SubscribeAsync<PaymentReceivedEvent>(
                PaymentReceivedChannel,
                async (paymentEvent) => await processPaymentUseCase.Handle(paymentEvent)
            );

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
