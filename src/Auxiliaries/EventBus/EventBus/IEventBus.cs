using EventBus.Events;

namespace EventBus
{
    public interface IEventBus
    {
        void Publish(IntegrationEventBase evt);
    }
}
