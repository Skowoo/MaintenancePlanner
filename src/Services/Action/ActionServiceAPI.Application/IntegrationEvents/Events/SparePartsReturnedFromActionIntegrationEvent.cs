using ActionServiceAPI.Domain.Models;
using EventBus.Events;

namespace ActionServiceAPI.Application.IntegrationEvents.Events
{
    public record SparePartsReturnedFromActionIntegrationEvent(List<UsedPart> UsedParts) : IntegrationEventBase
    {
        public List<UsedPart> UsedParts { get; init; } = UsedParts;
    }
}
