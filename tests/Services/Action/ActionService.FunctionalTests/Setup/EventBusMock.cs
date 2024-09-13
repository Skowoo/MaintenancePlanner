using EventBus.Abstractions;
using EventBus.Events;

namespace ActionService.FunctionalTests.Setup
{
    public class EventBusMock : IEventBus
    {
        public void Publish(IntegrationEventBase evt)
        {
            
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEventBase
            where TH : IIntegrationEventHandler<T>
        {
            
        }
    }
}
