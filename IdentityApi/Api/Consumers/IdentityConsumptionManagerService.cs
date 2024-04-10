using Core.RabbitLogic;
using Core.RabbitLogic.Interfaces;

namespace Api.Consumers;

public class IdentityConsumptionManagerService(IEnumerable<IConsumer> consumers)
    : BaseRabbitMqConsumptionManagerService(consumers)
{
    protected override string ResponseExchangeName => "IdentityApiResponse";
}