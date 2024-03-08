namespace Shared.Dto.GroupRoles;

public record GroupRoleResponse(Guid Id, 
    Guid? GroupId, 
    bool IsUnique,
    bool IsDefault,
    bool IsRevocable, 
    string Name, 
    string Description,
    IEnumerable<Guid> GroupRoleRights);