using EventBus.Events;

namespace WarehouseServiceAPI.IntegrationEvents
{
    public interface IIntegrationEventService
    {
        Task PublishIntegrationEventAsync(IntegrationEventBase evt);
    }
}
