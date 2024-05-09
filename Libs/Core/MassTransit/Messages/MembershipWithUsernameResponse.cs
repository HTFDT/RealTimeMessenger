namespace Core.MassTransit.Messages;

public record MembershipWithUsernameResponse
{
    public string Username { get; init; } = null!;
    public MembershipResponse Membership { get; init; } = null!;
}