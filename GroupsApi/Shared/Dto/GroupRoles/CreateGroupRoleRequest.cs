namespace Shared.Dto.GroupRoles;

public record CreateGroupRoleRequest(string Name,
    string Description, 
    bool IsUnique,
    IEnumerable<Guid> GroupRights);