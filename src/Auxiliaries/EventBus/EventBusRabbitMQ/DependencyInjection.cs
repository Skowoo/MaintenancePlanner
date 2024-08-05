using EventBus.Abstractions;
using EventBus;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace EventBusRabbitMQ
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRabbitMQEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IPersistentConnection, PersistentConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost"
                };
                return new PersistentConnection(factory);
            });
            services.AddSingleton<ISubscriptionManager, SubscriptionManager>();
            services.AddSingleton<IEventBus, EventBusService>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<IServiceProvider>();
                var eventBusSubscriptionsManager = sp.GetRequiredService<ISubscriptionManager>();

                return new EventBusService(rabbitMQPersistentConnection, eventBusSubscriptionsManager, iLifetimeScope);
            });

            return services;
        }
    }
}
