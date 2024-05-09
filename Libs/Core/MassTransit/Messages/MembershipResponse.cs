namespace Core.MassTransit.Messages;

public record MembershipResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid GroupId { get; init; }
    public Guid? GroupRoleId { get; init; }
    public DateTime DateJoined { get; init; }
    public bool IsKicked { get; init; }
    public DateTime? DateKicked { get; init; }
}