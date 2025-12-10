using Application.DTOs;
using Domain.Entities;
using Domain.Events;
using Domain.Services;

namespace Application.UseCases;

public class ReceivePaymentUseCase(IMessageBusService messageBusService)
{
    private const string PaymentReceivedChannel = "payment:received";

    public async Task Handle(ReceivePaymentRequest request)
    {
        var paymentEvent = new PaymentReceivedEvent
        {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        await messageBusService.PublishAsync(PaymentReceivedChannel, paymentEvent);
    }
}
