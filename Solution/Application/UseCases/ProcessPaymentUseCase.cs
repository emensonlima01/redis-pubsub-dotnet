using Domain.Entities;
using Domain.Events;
using Domain.Repositories;

namespace Application.UseCases;

public class ProcessPaymentUseCase(IPaymentRepository paymentRepository)
{
    public async Task Handle(PaymentReceivedEvent paymentEvent)
    {
        var payment = new Payment
        {
            Id = paymentEvent.Id,
            Amount = paymentEvent.Amount,
            Description = paymentEvent.Description,
            CreatedAt = paymentEvent.CreatedAt,
            Status = PaymentStatus.Received
        };

        await paymentRepository.SaveAsync(payment);
    }
}
