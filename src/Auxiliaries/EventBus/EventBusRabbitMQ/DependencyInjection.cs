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
            // Register all integration event handlers
            var handlerType = typeof(IIntegrationEventHandler<>);
            var handlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType))
                .ToList();

            foreach (var handler in handlers)
                services.AddTransient(handler);

            // Register event bus services
            services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost"
                };
                return new RabbitMQConnection(factory);
            });
            services.AddSingleton<IHandlersManager, HandlersManager>();
            services.AddSingleton<IEventBus, EventBusService>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQConnection>();
                var iLifetimeScope = sp.GetRequiredService<IServiceProvider>();
                var eventBusSubscriptionsManager = sp.GetRequiredService<IHandlersManager>();

                return new EventBusService(rabbitMQPersistentConnection, eventBusSubscriptionsManager, iLifetimeScope);
            });

            return services;
        }
    }
}
