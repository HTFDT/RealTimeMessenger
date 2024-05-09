using Core.MassTransit.Messages;
using MassTransit;

namespace web.Sagas.Memberships;

public class MembershipState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public State CurrentState { get; set; } = null!;
    public Guid? RequestId { get; set; }

    public MembershipResponse MembershipResponse { get; set; } = null!;
    public Uri? ResponseAddress { get; set; }
    public Guid? ProcessingId { get; set; }
}