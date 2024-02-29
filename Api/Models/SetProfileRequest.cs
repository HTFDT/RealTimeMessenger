using Dal.Enums;

namespace Api.Models;

public record SetProfileRequest
{
    public required string? ProfileDescription { get; init; }
    public required Gender? Gender { get; init; }
}