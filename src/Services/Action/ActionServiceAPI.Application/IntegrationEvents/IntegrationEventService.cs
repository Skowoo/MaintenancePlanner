using EventBus.Abstractions;
using EventBus.Events;

namespace ActionServiceAPI.Application.IntegrationEvents
{
    public class IntegrationEventService(IEventBus eventBus) : IIntegrationEventService
    {
        public void Publish(IntegrationEventBase evt) => eventBus.Publish(evt);
    }
}
