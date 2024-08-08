using EventBus.Abstractions;
using EventBus.Events;

namespace EventBus
{
    public interface IHandlersManager
    {
        void AddHandler<T, TH>()
            where T : IntegrationEventBase
            where TH : IIntegrationEventHandler<T>;

        string GetEventName<T>() where T : IntegrationEventBase;

        Type? GetEventTypeByName(string eventName);

        IEnumerable<Type> GetHandlersForEventName(string eventName);

        bool HasSubscriptionsForEvent(string eventName);
    }
}