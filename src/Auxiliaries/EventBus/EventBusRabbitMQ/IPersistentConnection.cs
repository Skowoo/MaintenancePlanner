using RabbitMQ.Client;

namespace EventBusRabbitMQ
{
    public interface IPersistentConnection
    {
        bool IsConnected { get; }

        IModel CreateModel();

        bool TryConnect();
    }
}