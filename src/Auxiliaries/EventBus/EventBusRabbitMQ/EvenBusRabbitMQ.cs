using EventBus;
using EventBus.Events;
using RabbitMQ.Client;
using System.Text.Json;

namespace EventBusRabbitMQ
{
    public class EvenBusRabbitMQ : IEventBus
    {
        public void Publish(IntegrationEventBase evt)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "EventBus",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            var body = JsonSerializer.SerializeToUtf8Bytes(evt);

            var eventName = evt.GetType().Name;

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "EventBus", // Queue name
                                 basicProperties: null,
                                 body: body);
        }
    }
}
