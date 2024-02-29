namespace Api.Models;

public record CreateRoleRequest
{
    public required string RoleName { get; init; }
}