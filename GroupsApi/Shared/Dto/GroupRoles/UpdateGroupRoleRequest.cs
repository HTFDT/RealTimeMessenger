namespace Shared.Dto.GroupRoles;

public record UpdateGroupRoleRequest(string Name,
    string Description,
    IEnumerable<Guid> GroupRights);