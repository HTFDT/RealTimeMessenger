using RabbitMQ.Client;

namespace Core.RabbitLogic.Interfaces;

public interface IConsumer
{
    public void Consume(IConnection connection, string exchange, string routingKey, string exchangeType);
}