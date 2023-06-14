using RabbitMqExample.Common.Models;
using RabbitMqExample.Common.Services;

namespace RabbitMqExample.ConsumerConsole1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var settings = new RabbitMqSettings
            {
                Host = "165.227.158.6",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            MessageService<Booking> messageService = new MessageService<Booking>(settings);
            var tx = messageService.ReceiveMessage();

            

            Console.ReadLine();
        }


    }
}