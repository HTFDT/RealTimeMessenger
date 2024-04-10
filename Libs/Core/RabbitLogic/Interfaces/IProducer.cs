using RabbitMQ.Client;

namespace Core.RabbitLogic.Interfaces;

public interface IProducer
{
    public Task<string> ProduceAsync(IConnection connection, string body, string exchange, string routingKey, string exchangeType, string replyTo, CancellationToken token = default);
}