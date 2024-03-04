namespace Logic.Models.User;

public record UserOutModel
{
    public required Guid Id { get; init; } 
    public required string Username { get; init; } 
}