using EventBus.Events;

namespace EventBus.Abstractions
{
    public interface IEventBus
    {
        void Publish(IntegrationEventBase evt);

        void Subscribe<T, TH>()
            where T : IntegrationEventBase
            where TH : IIntegrationEventHandler<T>;
    }
}
