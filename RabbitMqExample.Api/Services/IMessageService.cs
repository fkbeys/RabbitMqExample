namespace RabbitMqExample.Api.Services
{
    public interface IMessageService
    {
        public void SendMessage<T>(T message);

    }
}
