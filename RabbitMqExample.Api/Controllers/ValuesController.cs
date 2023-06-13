using Microsoft.AspNetCore.Mvc;
using RabbitMqExample.Api.Models;
using RabbitMqExample.Api.Services;

namespace RabbitMqExample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public ValuesController(IMessageService messageService)
        {
            this._messageService = messageService;
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
