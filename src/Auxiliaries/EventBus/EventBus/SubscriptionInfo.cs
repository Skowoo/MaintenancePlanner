namespace EventBus
{
    public class SubscriptionInfo(bool isDynamic, Type handler)
    {
        public bool IsDybamic { get; } = isDynamic;

        public Type HandlerType { get; } = handler;

        public static SubscriptionInfo Dynamic(Type handler) => new(true, handler);

        public static SubscriptionInfo Typed(Type handler) => new(false, handler);
    }
}
