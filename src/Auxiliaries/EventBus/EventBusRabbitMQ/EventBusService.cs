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
        const string EXCHANGE_NAME = "maintenance_planner_event_bus";

        readonly IRabbitMQConnection _persistentConnection;
        readonly IHandlersManager _subscriptionManager;
        readonly IServiceProvider _serviceProvider;
        readonly IModel _consumerChannel;

        public EventBusService(IRabbitMQConnection persistentConnection, IHandlersManager subscriptionManager, IServiceProvider serviceProvider)
        {
            _persistentConnection = persistentConnection;
            _subscriptionManager = subscriptionManager;
            _serviceProvider = serviceProvider;

            _persistentConnection.TryConnect();
            _consumerChannel = _persistentConnection.CreateModel();
        }

        public void Publish(IntegrationEventBase evt)
        {
            var eventName = evt.GetType().Name;

            using var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: EXCHANGE_NAME, type: "direct");

            var body = JsonSerializer.SerializeToUtf8Bytes(evt, evt.GetType());

            channel.BasicPublish(
                exchange: EXCHANGE_NAME,
                routingKey: eventName,
                body: body);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEventBase
            where TH : IIntegrationEventHandler<T>
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            var eventName = _subscriptionManager.GetEventName<T>();

            _consumerChannel.ExchangeDeclare(exchange: EXCHANGE_NAME, type: "direct");

            var subscriberQueueName = _consumerChannel.QueueDeclare().QueueName;
            _consumerChannel.QueueBind(
                queue: subscriberQueueName,
                exchange: EXCHANGE_NAME,
                routingKey: eventName);

            var consumer = new EventingBasicConsumer(_consumerChannel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                await ProcessEvent(eventName, message);
                consumer.Model.BasicAck(ea.DeliveryTag, multiple: false);
            };

            _consumerChannel.BasicConsume(
                queue: subscriberQueueName,
                autoAck: false,
                consumer: consumer);

            _subscriptionManager.AddHandler<T, TH>();
        }

        async Task ProcessEvent(string eventName, string message)
        {
            if (!_subscriptionManager.HasSubscriptionsForEvent(eventName))
                return;

            await using var scope = _serviceProvider.CreateAsyncScope();
            var subscriptions = _subscriptionManager.GetHandlersForEventName(eventName);
            foreach (var handlerType in subscriptions)
            {
                var handler = scope.ServiceProvider.GetService(handlerType);

                if (handler is null)
                    continue;

                var eventType = _subscriptionManager.GetEventTypeByName(eventName);
                var integrationEvent = JsonSerializer.Deserialize(message, eventType!, new JsonSerializerOptions() 
                { 
                    PropertyNameCaseInsensitive = true 
                });

                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType!);
                await Task.Yield();
                await (Task)concreteType.GetMethod("Handle")!.Invoke(handler, [integrationEvent!])!;
            }
        }
    }
}