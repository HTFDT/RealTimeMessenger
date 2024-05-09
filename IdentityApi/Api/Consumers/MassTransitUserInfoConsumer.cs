using Core.MassTransit.Messages;
using Logic;
using MassTransit;

namespace Api.Consumers;

public class MassTransitUserInfoConsumer(UserManager manager) : IConsumer<UserInfoRequest>
{
    public async Task Consume(ConsumeContext<UserInfoRequest> context)
    {
        var user = await manager.GetById(context.Message.Id);
        if (user is null)
            throw new InvalidOperationException("User not found");
        await context.RespondAsync(new UserInfoResponse { Id = user.Id, Username = user.Username });
    }
}