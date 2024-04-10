using Core.RabbitLogic;
using Core.RabbitLogic.Interfaces;

namespace Api.Consumers;

public abstract class BaseIdentityConsumer<TRequest, TResponse> : BaseRabbitMqServerConsumer<TRequest, TResponse>
{
    protected override string ResponseExchangeName => "IdentityApiResponse";
}