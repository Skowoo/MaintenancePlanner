using EventBus.Events;

namespace WarehouseServiceAPI.IntegrationEvents.Events
{
    public record SparePartsUsedInActionIntegrationEvent : IntegrationEventBase
    {
        public List<UsedPart> UsedParts { get; init; } = [];
    }

    public class UsedPart
    {
        public int Id { get; set; }

        public int PartId { get; set; }

        public int Quantity { get; set; }
    }
}
