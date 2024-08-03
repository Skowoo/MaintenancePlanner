using EventBus.Events;

namespace EventBus.Abstractions
{
    public interface IIntegrationEventHandler<T> where T : IntegrationEventBase
    {
        Task Handle(T evt);
    }
}
