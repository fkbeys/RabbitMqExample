using RabbitMQ.Client;

namespace RabbitMqExample.Common.Services
{
    public abstract class MessageAbstractService<T>
    {
        protected abstract string queueName();
        protected abstract string exchangeName();
        protected abstract (IConnection, IModel) Connect();
        public abstract void SendMessage(T message);
        public abstract T ReceiveMessage();
    }

   


}
