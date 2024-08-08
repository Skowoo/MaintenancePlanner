using EventBus.Abstractions;
using EventBus.Events;

namespace EventBus
{
    public class HandlersManager : IHandlersManager
    {
        readonly Dictionary<string, List<Type>> _handlers = [];
        readonly List<Type> _registeredEventTypes = [];

        public void AddHandler<T, TH>()
            where T : IntegrationEventBase
            where TH : IIntegrationEventHandler<T>
        {
            var eventType = typeof(T);
            var eventName = eventType.Name;

            if (!_registeredEventTypes.Contains(eventType))
                _registeredEventTypes.Add(eventType);

            if (!_handlers.TryGetValue(eventName, out List<Type>? value))
                _handlers.Add(eventName, [typeof(TH)]);
            else
                value.Add(typeof(TH));
        }

        public IEnumerable<Type> GetHandlersForEventName(string eventName)
            => _handlers[eventName];

        public bool HasSubscriptionsForEvent(string eventName)
            => _handlers.ContainsKey(eventName);

        public string GetEventName<T>() where T : IntegrationEventBase
            => typeof(T).Name;

        public Type? GetEventTypeByName(string eventName)
            => _registeredEventTypes.SingleOrDefault(t => t.Name == eventName);
    }
}
