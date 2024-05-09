namespace Core.MassTransit.Messages;

public record ProcessMembership
{
    public Guid RequesterId { get; init; }
    public Guid MembershipId { get; init; }
    public Guid GroupId { get; init; }
}