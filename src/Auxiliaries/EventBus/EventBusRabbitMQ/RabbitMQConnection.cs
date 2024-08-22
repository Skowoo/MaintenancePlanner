using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EventBusRabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        readonly IConnectionFactory _connectionFactory;
        readonly object connectionLock = new();
        readonly ILogger<RabbitMQConnection> _logger;
        IConnection _connection;

        public bool IsConnected => _connection is not null && _connection.IsOpen && !Disposed;

        public bool Disposed;

        public RabbitMQConnection(IConnectionFactory factory, ILogger<RabbitMQConnection> logger)
        {
            _connectionFactory = factory;
            _logger = logger;
            _connection = _connectionFactory.CreateConnection();
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
                throw new InvalidOperationException("Not connected!");

            _logger.LogTrace("Creating RabbitMQ channel");

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (Disposed)
                return;

            _logger.LogTrace("Disposing RabbitMQ channel");

            Disposed = true;

            _connection.Dispose();
        }

        public bool TryConnect()
        {
            _logger.LogTrace("RabbitMQ Client connecting to the server...");

            lock (connectionLock)
            {
                _connection = _connectionFactory.CreateConnection();

                if (IsConnected)
                {
                    _logger.LogTrace("RabbitMQ Client connected to the server!");
                    return true;
                }
            }

            _logger.LogError("RabbitMQ Client could not connect!");
            return false;
        }
    }
}