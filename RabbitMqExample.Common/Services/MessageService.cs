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

        protected override string queueName() => typeof(T).Name + "Queue";
        protected override string exchangeName() => typeof(T).Name + "Exchange";

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
                channel.QueueDeclare(queueName(), durable: true, exclusive: false);
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

                _channel.BasicConsume(queue: queueName(), autoAck: true, consumer: consumer);
            });
        }

        public override void SendMessage(T message)
        {
            _retryPolicy.Execute(() =>
            {
                var jsonString = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonString);
                _channel.BasicPublish(exchange: "", routingKey: queueName(), basicProperties: null, body: body);
            });
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }

}
