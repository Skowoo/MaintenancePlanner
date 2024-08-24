using EventBus.Events;

namespace WarehouseServiceAPI.IntegrationEvents.Events
{
    public record SparePartsReturnedFromActionIntegrationEvent(List<UsedPart> UsedParts) : IntegrationEventBase
    {
        public List<UsedPart> UsedParts { get; init; } = UsedParts;
    }
}
