using RabbitMQ.Client;

namespace EventBusRabbitMQ
{
    public class PersistentConnection(IConnectionFactory factory) : IPersistentConnection
    {
        readonly IConnectionFactory _connectionFactory = factory;
        IConnection _connection = null!;
        public bool Disposed;
        public bool IsConnected => _connection is not null && _connection.IsOpen && !Disposed;
        readonly object connectionLock = new();

        public IModel Create()
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