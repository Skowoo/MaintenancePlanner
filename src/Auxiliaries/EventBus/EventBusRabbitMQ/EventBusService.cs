using EventBus;
using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EventBusRabbitMQ
{
    public class EventBusService : IEventBus
    {
        const string EXCHANGE_NAME = "maintenance_planner_event_bus";

        readonly IRabbitMQConnection _rabbitMQConnection;
        readonly IHandlersManager _subscriptionManager;
        readonly IServiceProvider _serviceProvider;
        readonly ILogger<EventBusService> _logger;
        readonly IModel _consumerChannel;

        public EventBusService(IRabbitMQConnection connection,
            IHandlersManager subscriptionManager,
            IServiceProvider serviceProvider,
            ILogger<EventBusService> logger)
        {
            _rabbitMQConnection = connection;
            _subscriptionManager = subscriptionManager;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _consumerChannel = ConnectToChannel();
        }

        public void Publish(IntegrationEventBase evt)
        {
            if (!_rabbitMQConnection.IsConnected)
                _rabbitMQConnection.TryConnect();

            var eventName = evt.GetType().Name;

            _logger.LogTrace("Publishing {eventName}: (Id: {})", eventName, evt.Id);

            using var channel = _rabbitMQConnection.CreateModel();

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
            if (!_rabbitMQConnection.IsConnected)
                _rabbitMQConnection.TryConnect();

            _logger.LogInformation("Subscribing to {EventName} with {EventHandler}", typeof(T).Name, typeof(TH).Name);

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

        IModel ConnectToChannel()
        {
            _logger.LogInformation("Creating RabbitMQ consumer channel");

            _rabbitMQConnection.TryConnect();
            return _rabbitMQConnection.CreateModel();
        }

        async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing {EventName} started", eventName);

            if (!_subscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                _logger.LogWarning("No subscriptions for {EventName}", eventName);
                return;
            }

            await using var scope = _serviceProvider.CreateAsyncScope();
            var subscriptions = _subscriptionManager.GetHandlersForEventName(eventName);
            foreach (var handlerType in subscriptions)
            {
                string? eventId = null;
                try
                {
                    var handler = scope.ServiceProvider.GetService(handlerType);

                    if (handler is null)
                        continue;

                    var eventType = _subscriptionManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonSerializer.Deserialize(message, eventType!, GetJsonSerializerOptions());

                    eventId = (integrationEvent as IntegrationEventBase)?.Id.ToString()
                        ?? "IntegrationEventBase parsing failed, no Id retrieved.";

                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType!);
                    await Task.Yield();
                    await (Task)concreteType.GetMethod("Handle")!.Invoke(handler, [integrationEvent!])!;

                    _logger.LogTrace("Processed {EventName} (Id: {})", eventName, eventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing {EventName} Id: {}", eventName, eventId);
                    throw;
                }
            }
        }

        static JsonSerializerOptions GetJsonSerializerOptions() => new()
        {
            PropertyNameCaseInsensitive = true
        };
    }
}