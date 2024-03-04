namespace Api.Models;

public record GetUserResponse
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
}