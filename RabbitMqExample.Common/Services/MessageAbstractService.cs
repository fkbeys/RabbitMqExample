using RabbitMQ.Client;

namespace RabbitMqExample.Common.Services
{
    public abstract class MessageAbstractService<T>
    {
        protected abstract string QueueName();
        protected abstract string ExchangeName();
        protected abstract string RoutingKey();
        protected abstract (IConnection, IModel) Connect();
        public abstract void SendMessage(T message);
        public abstract void ReceiveMessage(Action<T> callback);
    }
     




}
