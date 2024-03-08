namespace Shared.Dto.GroupRoles;

public record CreateDefaultGroupRoleRequest(bool IsRevocable,
    bool IsUnique,
    bool IsDefaultMemberRole,
    bool IsDefaultKickedRole,
    bool IsDefaultOwnerRole,
    bool IsDefaultLeftRole,
    bool IsAssignableByUsers,
    string Name,
    string Description,
    IEnumerable<Guid> GroupRights);