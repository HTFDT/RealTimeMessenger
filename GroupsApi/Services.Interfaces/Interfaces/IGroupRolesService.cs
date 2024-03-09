using Shared.Dto.GroupRights;
using Shared.Dto.GroupRoles;
using Shared.Dto.Groups;

namespace Services.Interfaces.Interfaces;

public interface IGroupRolesService
{
    Task<IEnumerable<GroupRoleResponse>> GetGroupRoles(Guid requesterId, Guid groupId);
    Task<GroupRoleResponse> GetGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId);
    Task<GroupRoleResponse> CreateGroupRole(Guid requesterId, Guid groupId, CreateGroupRoleRequest request);
    Task<GroupRoleResponse> UpdateGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId, UpdateGroupRoleRequest request);
    Task DeleteGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId);
    Task<GroupRoleResponse> AddRightToGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId, AddRightToGroupRoleRequest request);
    Task<GroupRoleResponse> RemoveRightFromGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId, Guid groupRightId);
    Task<IEnumerable<GroupRoleResponse>> GetDefaultGroupRoles();
    Task<GroupRoleResponse> CreateDefaultGroupRole(CreateDefaultGroupRoleRequest request);
    Task<GroupRoleResponse> UpdateDefaultGroupRole(Guid defaultGroupRoleId, UpdateGroupRoleRequest request);
}