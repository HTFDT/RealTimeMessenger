namespace Api.Models;

public record RoleResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}