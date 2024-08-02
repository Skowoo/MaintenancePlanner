using EventBus.Events;

namespace ActionServiceAPI.Application.IntegrationEvents.Events
{
    public record NewPartAddedIntegrationEvent : IntegrationEventBase
    {
        public int PartId { get; init; }

        public int Quantity { get; init; }
    }
}
