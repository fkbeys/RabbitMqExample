using RabbitMQ.Client;
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

        protected override string queueName() => typeof(T).Name + "Queue";
        protected override string exchangeName() => typeof(T).Name + "Exchange";

        public MessageService(IRabbitMqSettings rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings;
            (_connection, _channel) = Connect();
        }

        protected override (IConnection, IModel) Connect()
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
        }

        public override T ReceiveMessage()
        {
            var result = _channel.BasicGet(queueName(), true);
            if (result == null)
                return default;

            var body = result.Body.ToArray();
            var jsonString = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<T>(jsonString);

            return message;
        }

        public override void SendMessage(T message)
        {
            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);
            _channel.BasicPublish(exchangeName(), queueName(), null, body);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }


    }

}
