using RabbitMqExample.Common.Models;
using RabbitMqExample.Common.Services;

namespace RabbitMqExample.ConsumerConsole2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Booking console app started");
            var settings = new RabbitMqSettings
            {
                Host = "165.227.158.6",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            MessageService<Booking> messageService = new MessageService<Booking>(settings);
            Console.WriteLine("Listening...");
            messageService.ReceiveMessage(consumeReceivedMessage);
            Console.ReadLine();
        }

        public static void consumeReceivedMessage(Booking booking)
        {
            Console.WriteLine("message received...");
            Console.WriteLine($"name:{booking.customerName},id:{booking.id},from:{booking.From},to:{booking.to}");
        }

    }
}