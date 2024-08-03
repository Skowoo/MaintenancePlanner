using EventBus.Abstractions;
using EventBus.Events;

namespace EventBus
{
    public interface ISubscriptionManager
    {
        bool IsEmpty { get; }

        void AddDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler;
        void AddSubscription<T, TH>()
            where T : IntegrationEventBase
            where TH : IIntegrationEventHandler<T>;
        void Clear();
        string GetEventKey<T>() where T : IntegrationEventBase;
        Type? GetEventTypeByName(string eventName);
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEventBase;
        bool HasSubscriptionsForEvent(string eventName);
        bool HasSubscriptionsForEvent<T>() where T : IntegrationEventBase;
        void RemoveDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler;
        void RemoveSubscription<T, TH>()
            where T : IntegrationEventBase
            where TH : IIntegrationEventHandler<T>;
    }
}