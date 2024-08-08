using RabbitMQ.Client;

namespace EventBusRabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        readonly IConnectionFactory _connectionFactory;
        readonly object connectionLock = new();
        IConnection _connection;

        public bool IsConnected => _connection is not null && _connection.IsOpen && !Disposed;

        public bool Disposed;

        public RabbitMQConnection(IConnectionFactory factory)
        {
            _connectionFactory = factory;
            _connection = _connectionFactory.CreateConnection();
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
                throw new InvalidOperationException("Not connected!");

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (Disposed)
                return;

            Disposed = true;

            _connection.Dispose();
        }

        public bool TryConnect()
        {
            lock (connectionLock)
            {
                _connection = _connectionFactory.CreateConnection();

                if (IsConnected)
                    return true;
            }
            return false;
        }
    }
}