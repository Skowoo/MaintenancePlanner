using EventBus.Events;

namespace IdentityServiceAPI.IntegrationEvents.Events
{
    public record NewUserCreatedIntegrationEvent(string UserId) : IntegrationEventBase
    {
        public string UserId { get; init; } = UserId;
    }
}