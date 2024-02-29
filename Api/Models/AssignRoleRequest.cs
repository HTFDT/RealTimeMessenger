namespace Api.Models;

public record AssignRoleRequest
{
    public required string RoleName { get; init; }
}