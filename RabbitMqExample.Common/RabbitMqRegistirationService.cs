using Microsoft.Extensions.DependencyInjection;
using RabbitMqExample.Common.Models;
using RabbitMqExample.Common.Services;

namespace RabbitMqExample.Common
{
    public static class RabbitMqRegistirationService
    {
        public static void RabbitMqRegistiration(this IServiceCollection service, RabbitMqSettings rabbitMqSettings)
        {
            service.AddScoped(sp => new MessageService<Booking>(rabbitMqSettings));
        }

    }
}
