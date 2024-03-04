using Dal.Enums;

namespace Logic.Models.Profile;

public record ProfileModel
{
    public required string? ProfileDescription { get; init; }
    public required Gender? Gender { get; init; }
}