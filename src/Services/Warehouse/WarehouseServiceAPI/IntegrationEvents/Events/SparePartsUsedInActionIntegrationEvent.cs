using EventBus.Events;

namespace WarehouseServiceAPI.IntegrationEvents.Events
{
    public record SparePartsUsedInActionIntegrationEvent(List<UsedPart> UsedParts) : IntegrationEventBase
    {
        public List<UsedPart> UsedParts { get; init; } = UsedParts;
    }

    /// <summary>
    /// DTO class for easier handling of SparePartsUsedInActionIntegrationEvent
    /// </summary>
    public class UsedPart
    {
        public int Id { get; set; }

        public int PartId { get; set; }

        public int Quantity { get; set; }
    }
}
