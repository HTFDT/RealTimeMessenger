using System.Collections.Concurrent;
using System.Text;
using Core.RabbitLogic.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Core.RabbitLogic;


/// <summary>
/// синглтон, который хранит все текущие запросы к другим сервисам через брокер
/// </summary>
internal class RabbitMqClientService : IRabbitMqClientService
{
    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _tasks = new();
    
    public Task<string> ProduceAsync(IConnection connection, string body, string exchange, string routingKey, string exchangeType, string replyTo, CancellationToken token = default)
    {
        using var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange, exchangeType);

        var props = channel.CreateBasicProperties();
        props.ContentType = "application/json";
        props.ReplyTo = replyTo;
        
        var id = Guid.NewGuid().ToString();
        props.CorrelationId = id;
        var message = Encoding.UTF8.GetBytes(body);

        var tcs = new TaskCompletionSource<string>();
        _tasks.TryAdd(id, tcs);
        
        channel.BasicPublish(exchange, routingKey, props, message);
        
        token.Register(() => _tasks.TryRemove(id, out _));
        return tcs.Task;
    }

    public void Consume(IConnection connection, string exchange, string routingKey, string exchangeType)
    {
        // биндинги поддерживаются пока жив канал, канал закрвывается при закрытии подключения
        var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange, exchangeType);
        var qName = channel.QueueDeclare().QueueName;
        
        channel.QueueBind(qName, exchange, routingKey);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var id = ea.BasicProperties.CorrelationId;
            var message = Encoding.UTF8.GetString(body);
            if (!_tasks.TryRemove(id, out var tcs))
                return;
            tcs.SetResult(message);
        };

        channel.BasicConsume(qName, true, consumer);
    }
}