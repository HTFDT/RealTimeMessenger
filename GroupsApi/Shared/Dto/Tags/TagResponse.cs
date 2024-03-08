using Shared.Dto.Groups;

namespace Shared.Dto.Tags;

public record TagResponse(Guid Id, string Name, string Description);