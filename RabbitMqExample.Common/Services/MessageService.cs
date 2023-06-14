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
        //public override T ReceiveMessage()
        //{
        //    var taskCompletionSource = new TaskCompletionSource<T>();
        //    var consumer = new EventingBasicConsumer(_channel);

        //    consumer.Received += (sender, args) =>
        //    {
        //        var body = args.Body.ToArray();
        //        var message = Encoding.UTF8.GetString(body);
        //        T data = JsonSerializer.Deserialize<T>(message);

        //        taskCompletionSource.SetResult(data);
        //    };
        //    _channel.BasicConsume(queue: queueName(), autoAck: true, consumer: consumer);
        //    var sss = taskCompletionSource.Task.Result;
        //    return sss;
        //}

        //public override T ReceiveMessage()
        //{
        //    var taskCompletionSource = new TaskCompletionSource<T>();
        //    var consumer = new EventingBasicConsumer(_channel);

        //    consumer.Received += (sender, args) =>
        //    {
        //        var body = args.Body.ToArray();
        //        var message = Encoding.UTF8.GetString(body);
        //        T data = JsonSerializer.Deserialize<T>(message);

        //        taskCompletionSource.SetResult(data);
        //    };
        //    _channel.BasicConsume(queue: queueName(), autoAck: true, consumer: consumer);
        //    var sss = taskCompletionSource.Task.Result;
        //    return sss;
        //}

        public void test()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += Consumer_Received;
        }

        public override void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            throw new NotImplementedException();
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
