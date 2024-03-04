namespace Logic.Models;

public record RoleOutModel
{
    public required Guid Id { get; init; }
    public required string RoleName { get; init; }
}