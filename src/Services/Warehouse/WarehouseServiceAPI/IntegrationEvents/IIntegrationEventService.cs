using EventBus.Events;

namespace WarehouseServiceAPI.IntegrationEvents
{
    public interface IIntegrationEventService
    {
        void PublishIntegrationEvent(IntegrationEventBase evt);
    }
}
