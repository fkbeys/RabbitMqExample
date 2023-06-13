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
            // _messageService = new MessageService<Booking>(rabbitMqSettings);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var msg = new Booking { id = 1, customerName = "", };
            _messageService.SendMessage(msg);

            return Ok("");
        }

    }
}
