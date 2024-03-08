namespace Shared.Dto.Groups;

public record UpdateGroupRequest(string Name,
    string Description,
    bool IsPrivate,
    IEnumerable<Guid> GroupTagIds);