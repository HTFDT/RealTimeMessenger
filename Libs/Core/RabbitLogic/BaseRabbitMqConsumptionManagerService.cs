using Core.RabbitLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace Core.RabbitLogic;

/// <summary>
/// Базовый класс для фонового сервиса, отвечающего за приём сообщений с клиента на сервере
/// </summary>
public abstract class BaseRabbitMqConsumptionManagerService : IHostedService, IDisposable
{
    private readonly Lazy<IConnection> _connection;
    private readonly IEnumerable<IConsumer> _consumers;
    
    protected BaseRabbitMqConsumptionManagerService(IEnumerable<IConsumer> consumers)
    {
        _consumers = consumers;
        _connection = new Lazy<IConnection>(() =>
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            return factory.CreateConnection();
        });
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var channel = _connection.Value.CreateModel();
        channel.ExchangeDeclare(ResponseExchangeName, ExchangeType.Direct);
        foreach (var con in _consumers)
        {
            var t = con.GetType();
            var key = t.BaseType!.GetGenericArguments()[0].Name;
            con.Consume(_connection.Value, key, key, ExchangeType.Direct);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_connection.IsValueCreated) 
            _connection.Value.Close();
    }
    
    protected abstract string ResponseExchangeName { get; }
}