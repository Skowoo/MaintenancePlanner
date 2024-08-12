using EventBus.Events;

namespace ActionServiceAPI.Application.IntegrationEvents.Events
{
    public record NewUserCreatedIntegrationEvent(string UserId) : IntegrationEventBase
    {
        public string UserId { get; init; } = UserId;
    }
}