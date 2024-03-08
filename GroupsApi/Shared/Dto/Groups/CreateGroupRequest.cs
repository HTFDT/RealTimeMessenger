namespace Shared.Dto.Groups;

public record CreateGroupRequest(string Name,
    string Description,
    bool IsPrivate,
    IEnumerable<Guid> GroupTagIds);