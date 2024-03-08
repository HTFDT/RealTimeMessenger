namespace Shared.Dto.Groups;

public record GroupResponse(Guid Id,
    string Name,
    string Description, 
    bool IsPrivate, 
    Guid? DefaultMemberRoleId,
    IEnumerable<Guid> GroupTagIds);