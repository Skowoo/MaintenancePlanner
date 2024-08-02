using EventBus;
using EventBus.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EventBusRabbitMQ
{
    public class EvenBusRabbitMQ : IEventBus
    {
        const string ExchangeName = "EventBus";

        public void Publish(IntegrationEventBase evt)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Fanout);

            var body = JsonSerializer.SerializeToUtf8Bytes(evt);

            var eventName = evt.GetType().Name;

            channel.BasicPublish(exchange: ExchangeName, // event name as routing key?
                                 routingKey: string.Empty, // should be eventName?
                                 basicProperties: null,
                                 body: body);
        }

        public IntegrationEventBase Receiver() // Refactor. Refactor all of this...
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Fanout);

            // declare a server-named queue
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: ExchangeName, // should be eventName?
                              routingKey: string.Empty);

            var consumer = new EventingBasicConsumer(channel);

            IntegrationEventBase receivedEvent = null;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize(Encoding.UTF8.GetString(body), typeof(IntegrationEventBase));   
                receivedEvent = (IntegrationEventBase)message!;
                
            };
            channel.BasicConsume(queue: queueName,
                     autoAck: true,
                     consumer: consumer);

            return receivedEvent!;
        }
    }
}
