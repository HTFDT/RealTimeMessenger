using Core.MassTransit.Messages;
using MassTransit;
using Services.Interfaces.Interfaces;

namespace Presentation.Consumers;

public class MassTransitMembershipConsumer(IServicesManager manager) : IConsumer<MembershipRequest>
{
    public async Task Consume(ConsumeContext<MembershipRequest> context)
    {
        var member = await manager.Memberships.GetMember(context.Message.RequesterId, context.Message.GroupId,
            context.Message.MembershipId);
        await context.RespondAsync(new MembershipResponse
        {
            Id = member.Id,
            UserId = member.UserId,
            GroupId = member.GroupId,
            GroupRoleId = member.GroupRoleId,
            DateJoined = member.DateJoined,
            IsKicked = member.IsKicked,
            DateKicked = member.DateKicked
        });
    }
}