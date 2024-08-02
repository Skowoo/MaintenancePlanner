using EventBus.Events;

namespace WarehouseServiceAPI.IntegrationEvents.Events
{
    public record NewPartAddedIntegrationEvent : IntegrationEventBase
    {
        public int PartId { get; init; }

        public int Quantity { get; init; }
    }
}
