using EventBus.Abstractions;
using EventBus.Events;

namespace EventBusRabbitMQ
{
    public class EventBusService : IEventBus
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
