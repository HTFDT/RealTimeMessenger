using System.Text;
using System.Text.Json;
using Core.RabbitLogic.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Core.RabbitLogic;

/// <summary>
/// Базовый класс для потребителя на стороне сервера (сервиса, обрабатывающего входящий запрос от другого сервиса)
/// </summary>
/// <typeparam name="TRequest">DTO запроса</typeparam>
/// <typeparam name="TResponse">DTO ответа</typeparam>
public abstract class BaseRabbitMqServerConsumer<TRequest, TResponse> : IConsumer
{
    public void Consume(IConnection connection, string exchange, string routingKey, string exchangeType)
    {
        var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange, exchangeType);
        var qName = channel.QueueDeclare().QueueName;
        
        channel.QueueBind(qName, exchange, routingKey);
        channel.BasicQos(0, 1, false);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (sender, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            var result = await HandleMessage(JsonSerializer.Deserialize<TRequest>(message)!);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result));

            var props = channel.CreateBasicProperties();
            props.CorrelationId = ea.BasicProperties.CorrelationId;

            channel.BasicPublish(ResponseExchangeName, ea.BasicProperties.ReplyTo, props, body);
            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume(qName, false, consumer);
    }
    
    /// <summary>
    /// метод для обработки сообщения реализацией потребителя
    /// </summary>
    protected abstract Task<TResponse?> HandleMessage(TRequest request);
    /// <summary>
    /// свойство, представляющее имя эксчейнджа, который используется для отправки ответа с сервера на клиент
    /// </summary>
    protected abstract string ResponseExchangeName { get; }
}