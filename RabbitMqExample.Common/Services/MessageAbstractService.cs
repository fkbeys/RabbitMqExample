using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqExample.Common.Services
{
    public abstract class MessageAbstractService<T>
    {
        protected abstract string queueName();
        protected abstract string exchangeName();
        protected abstract (IConnection, IModel) Connect();
        public abstract void SendMessage(T message);
        //public abstract T ReceiveMessage();
        public abstract void Consumer_Received(object? sender, BasicDeliverEventArgs e);
    }




}
