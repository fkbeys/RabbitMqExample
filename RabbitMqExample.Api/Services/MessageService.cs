using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMqExample.Api.Services
{
    public class MessageService : IMessageService
    {
        public MessageService()
        {

        }
        public void SendMessage<T>(T message)
        {

            var factory = new ConnectionFactory
            {
                HostName = "165.227.158.6",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
            };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("booking", durable: true, exclusive: false);

            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish("", "booking", null, body);
        }
    }
}
