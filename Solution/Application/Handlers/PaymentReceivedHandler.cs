using Application.UseCases;
using Domain.Events;
using Domain.Services;

namespace Application.Handlers;

public class PaymentReceivedHandler(ProcessPaymentUseCase useCase) : IEventHandler
{
    async Task IEventHandler.Handle(object message)
    {
        var paymentEvent = (PaymentReceivedEvent)message;
        await useCase.Handle(paymentEvent);
    }
}
