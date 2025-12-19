namespace Domain.Services;

public interface IEventHandler
{
    Task Handle(object message);
}