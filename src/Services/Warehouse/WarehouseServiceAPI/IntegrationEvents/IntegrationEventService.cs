using EventBus.Abstractions;
using EventBus.Events;

namespace WarehouseServiceAPI.IntegrationEvents
{
    public class IntegrationEventService(IEventBus eventBus) : IIntegrationEventService
    {
        public Task PublishIntegrationEventAsync(IntegrationEventBase evt)
        {
            eventBus.Publish(evt);
            return Task.CompletedTask;
        }
    }
}
