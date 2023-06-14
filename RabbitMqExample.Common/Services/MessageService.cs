using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqExample.Common.Models;
using System.Text;
using System.Text.Json;

namespace RabbitMqExample.Common.Services
{
    public class MessageService<T> : MessageAbstractService<T>, IDisposable
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly IRabbitMqSettings _rabbitMqSettings;
        private readonly Policy _retryPolicy;
        private readonly string _queueName;
        private readonly string _exchangeName;
        private readonly string _routingKey;

        protected override string QueueName() => _queueName;
        protected override string ExchangeName() => _exchangeName;
        protected override string RoutingKey() => _routingKey;

        public MessageService(IRabbitMqSettings rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings;
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(new[]
                {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
                });
            _queueName = typeof(T).Name + "Queue" + Guid.NewGuid().ToString();
            _exchangeName = typeof(T).Name + "Exchange";
            _routingKey = "";
            (_connection, _channel) = Connect();
        }

        protected override (IConnection, IModel) Connect()
        {
            return _retryPolicy.Execute(() =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = _rabbitMqSettings.Host,
                    Port = _rabbitMqSettings.Port,
                    UserName = _rabbitMqSettings.UserName,
                    Password = _rabbitMqSettings.Password
                };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                channel.ExchangeDeclare(exchange: _exchangeName, type: "fanout");
                channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false);
                channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: _routingKey);
                return (connection, channel);
            });
        }

        public override void ReceiveMessage(Action<T> callback)
        {
            _retryPolicy.Execute(() =>
            {
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (sender, args) =>
                {
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    T data = JsonSerializer.Deserialize<T>(message);

                    callback(data);
                };

                _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            });
        }

        public override void SendMessage(T message)
        {
            _retryPolicy.Execute(() =>
            {
                var jsonString = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonString);
                _channel.BasicPublish(exchange: _exchangeName, routingKey: _routingKey, basicProperties: null, body: body);
            });
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }

}
