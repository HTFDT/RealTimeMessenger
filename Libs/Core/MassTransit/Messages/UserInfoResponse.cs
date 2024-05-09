namespace Core.MassTransit.Messages;

public record UserInfoResponse
{
    public Guid Id { get; init; }
    public string Username { get; init; } = null!;
}