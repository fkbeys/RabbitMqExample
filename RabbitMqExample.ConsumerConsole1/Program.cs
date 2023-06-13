using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace RabbitMqExample.ConsumerConsole1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


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

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += Consumer_Received;


            Console.ReadLine();
        }

        private static void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            var messageJson = e.Body;
            var mesasge = JsonSerializer.Deserialize<(messageJson);


            Console.WriteLine(e.Body);
        }
    }
}