using EventBus;
using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EventBusRabbitMQ
{
    public class EventBusService : IEventBus
    {
        const string BROKER_NAME = "event_bus";
        const string AUTOFAC_SCOPE_NAME = "maintenance_planner_event_bus";

        readonly IPersistentConnection _persistentConnection;
        readonly ISubscriptionManager _subscriptionManager;
        readonly IServiceProvider _serviceProvider;
        readonly IModel _consumerChannel;
        readonly string _queueName;

        public EventBusService(IPersistentConnection persistentConnection, ISubscriptionManager subscriptionManager, IServiceProvider serviceProvider, string queueName = "event_bus_queue")
        {
            _persistentConnection = persistentConnection;
            _subscriptionManager = subscriptionManager;
            _serviceProvider = serviceProvider;
            _queueName = queueName;

            _persistentConnection.TryConnect();
            _consumerChannel = _persistentConnection.CreateModel();
        }

        public void Publish(IntegrationEventBase evt)
        {
            var eventName = evt.GetType().Name;

            _consumerChannel.ExchangeDeclare(exchange: eventName, type: "fanout");

            var body = JsonSerializer.SerializeToUtf8Bytes(evt, evt.GetType());

            _consumerChannel.BasicPublish(
                exchange: eventName,
                routingKey: string.Empty);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEventBase
            where TH : IIntegrationEventHandler<T>
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            var eventName = _subscriptionManager.GetEventKey<T>();

            _consumerChannel.ExchangeDeclare(exchange: eventName, type: "fanout");

            var subscriberQueueName = _consumerChannel.QueueDeclare().QueueName;
            _consumerChannel.QueueBind(
                queue: subscriberQueueName, 
                exchange: eventName, 
                routingKey: string.Empty);

            var consumer = new EventingBasicConsumer(_consumerChannel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                await ProcessEvent(eventName, message);
            };

            _consumerChannel.BasicConsume(
                queue: subscriberQueueName, 
                autoAck: true, 
                consumer: consumer);

            _subscriptionManager.AddSubscription<T, TH>();
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var subscriptions = _subscriptionManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        if (scope.ServiceProvider.GetService(subscription.HandlerType) is not IDynamicIntegrationEventHandler handler) 
                            continue;

                        using dynamic eventData = JsonDocument.Parse(message);
                        await Task.Yield();
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        var handler = scope.ServiceProvider.GetService(subscription.HandlerType);

                        if (handler is null) 
                            continue;

                        var eventType = _subscriptionManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }
            }
        }
    }
}
