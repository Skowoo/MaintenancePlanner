using RabbitMQ.Client;

namespace EventBusRabbitMQ
{
    public interface IRabbitMQConnection
    {
        bool IsConnected { get; }

        IModel CreateModel();

        bool TryConnect();
    }
}