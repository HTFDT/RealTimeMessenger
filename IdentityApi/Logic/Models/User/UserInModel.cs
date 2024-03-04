namespace Logic.Models.User;

public record UserInModel
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}