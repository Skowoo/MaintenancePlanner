using EventBus.Events;

namespace IdentityServiceAPI.IntegrationEvents
{
    public interface IIntegrationEventService
    {
        void PublishIntegrationEvent(IntegrationEventBase evt);
    }
}