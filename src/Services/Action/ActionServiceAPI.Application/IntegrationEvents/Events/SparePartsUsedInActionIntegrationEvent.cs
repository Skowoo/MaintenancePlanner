using ActionServiceAPI.Domain.Models;
using EventBus.Events;

namespace ActionServiceAPI.Application.IntegrationEvents.Events
{
    public record SparePartsUsedInActionIntegrationEvent : IntegrationEventBase
    {
        public List<UsedPart> UsedParts { get; init; } = [];
    }
}
