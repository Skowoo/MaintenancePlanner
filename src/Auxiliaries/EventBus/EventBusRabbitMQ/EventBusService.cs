using Autofac;
using EventBus;
using EventBus.Abstractions;
using EventBus.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EventBusRabbitMQ
{
    public class EventBusService(
        IPersistentConnection persistentConnection, 
        ISubscriptionManager subscriptionManager,
        ILifetimeScope autofac,
        string queueName = "event_bus_queue") : IEventBus
    {
        const string BROKER_NAME = "event_bus";
        const string AUTOFAC_SCOPE_NAME = "eshop_event_bus";

        readonly IPersistentConnection _persistentConnection = persistentConnection;
        readonly ISubscriptionManager _subscriptionManager = subscriptionManager;
        readonly ILifetimeScope _autofac = autofac;

        IModel _consumerChannel;
        string _queueName = queueName;

        public void Publish(IntegrationEventBase evt)
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();
            
            var eventName = evt.GetType().Name;

            using var channel = _persistentConnection.Create();

            channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");

            var body = JsonSerializer.SerializeToUtf8Bytes(evt, evt.GetType());

            var properties = channel.CreateBasicProperties();

            //properties.DeliveryMode = 2; // persistent

            channel.BasicPublish(
                exchange: BROKER_NAME,
                routingKey: eventName,
                mandatory: true,
                basicProperties: properties,
                body: body);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEventBase
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subscriptionManager.GetEventKey<T>();

            var containsKey = _subscriptionManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
                if (!_persistentConnection.IsConnected)
                    _persistentConnection.TryConnect();

            _consumerChannel.QueueBind(queue: _queueName,
                    exchange: BROKER_NAME,
                    routingKey: eventName);

            _subscriptionManager.AddSubscription<T, TH>();

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                //consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();
            
            var channel = _persistentConnection.Create();

            channel.ExchangeDeclare(exchange: BROKER_NAME,
                                    type: "direct");

            channel.QueueDeclare(queue: _queueName,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        private void StartBasicConsume()
        {
            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            if (message.ToLowerInvariant().Contains("throw-fake-exception"))
            {
                throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
            }

            await ProcessEvent(eventName, message);

            // Even on exception we take the message off the queue.
            // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
            // For more information see: https://www.rabbitmq.com/dlx.html
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                await using var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME);
                var subscriptions = _subscriptionManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        if (scope.ResolveOptional(subscription.HandlerType) is not IDynamicIntegrationEventHandler handler) continue;
                        using dynamic eventData = JsonDocument.Parse(message);
                        await Task.Yield();
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        var handler = scope.ResolveOptional(subscription.HandlerType);
                        if (handler == null) continue;
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
