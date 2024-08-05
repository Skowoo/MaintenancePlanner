using EventBus.Abstractions;
using EventBus.Events;

namespace EventBus
{
    public class SubscriptionManager : ISubscriptionManager
    {
        readonly Dictionary<string, List<SubscriptionInfo>> _handlers = []; // Holds collection of event handlers for each event type
        readonly List<Type> _eventTypes = []; // Holds collection of event registered types

        public bool IsEmpty => _handlers.Count == 0;
        public void Clear() => _handlers.Clear();

        public void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
                => DoAddSubscription(typeof(TH), eventName, true);

        public void AddSubscription<T, TH>()
            where T : IntegrationEventBase
            where TH : IIntegrationEventHandler<T>
        {
            DoAddSubscription(typeof(TH), typeof(T).Name, false);

            if (!_eventTypes.Contains(typeof(T)))
                _eventTypes.Add(typeof(T));
        }

        void DoAddSubscription(Type eventType, string eventName, bool isDynamic)
        {
            if (!_handlers.ContainsKey(eventName))
                _handlers.Add(eventName, []);

            if (isDynamic)
                _handlers[eventName].Add(SubscriptionInfo.Dynamic(eventType));
            else
                _handlers[eventName].Add(SubscriptionInfo.Typed(eventType));
        }

        public void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            var handlerToRemove = _handlers[eventName].SingleOrDefault(s => s.HandlerType == typeof(TH));
            if (handlerToRemove is not null)
                DoRemoveHandler(eventName, handlerToRemove);
        }

        public void RemoveSubscription<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEventBase
        {
            var handlerToRemove = _handlers[typeof(T).Name].SingleOrDefault(s => s.HandlerType == typeof(TH));
            if (handlerToRemove is not null)
                DoRemoveHandler(typeof(T).Name, handlerToRemove);
        }

        void DoRemoveHandler(string eventName, SubscriptionInfo toRemove) // Remove handler from event
        {
            if (toRemove is null)
                return;

            _handlers[eventName].Remove(toRemove); // Remove ggiven handler from event

            if (_handlers[eventName].Count != 0) // If there are no more handlers for the event
                return;

            _handlers.Remove(eventName); // Remove key from dictionary

            var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);

            if (eventType is not null) // If event type is found
                _eventTypes.Remove(eventType); // Remove event type from list
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>()
            where T : IntegrationEventBase
            => GetHandlersForEvent(GetEventKey<T>());

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName)
            => _handlers[eventName];

        public bool HasSubscriptionsForEvent<T>()
            where T : IntegrationEventBase
            => HasSubscriptionsForEvent(GetEventKey<T>());

        public bool HasSubscriptionsForEvent(string eventName)
            => _handlers.ContainsKey(eventName);

        public string GetEventKey<T>() where T : IntegrationEventBase
            => typeof(T).Name;

        public Type? GetEventTypeByName(string eventName)
            => _eventTypes.SingleOrDefault(t => t.Name == eventName);
    }
}
