using Microsoft.AspNetCore.Mvc;
using RabbitMqExample.Common.Models;
using RabbitMqExample.Common.Services;

namespace RabbitMqExample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly MessageService<Booking> _messageService;

        public ValuesController(MessageService<Booking> messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public IActionResult SendData(Booking bookingInfo)
        {
            _messageService.SendMessage(bookingInfo);

            return Ok("");
        }

    }
}
