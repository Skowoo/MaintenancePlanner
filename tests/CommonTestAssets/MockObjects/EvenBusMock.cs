using EventBus.Abstractions;
using EventBus.Events;

namespace CommonTestAssets.MockObjects
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
