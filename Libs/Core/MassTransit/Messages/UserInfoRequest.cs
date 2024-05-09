namespace Core.MassTransit.Messages;

public record UserInfoRequest
{
    public Guid Id { get; init; }
}