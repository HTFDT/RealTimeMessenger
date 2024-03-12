using Domain.Interfaces;
using Services.Interfaces.Interfaces;
using Shared.Dto.GroupRoles;

namespace Services.Decorators;

internal class GroupRolesServiceEnsureRightsDecorator(IGroupRolesService decoratee, IRepositoryManager repoManager) 
    : BaseEnsureRightsDecorator(repoManager), IGroupRolesService
{
    public async Task<IEnumerable<GroupRoleResponse>> GetGroupRoles(Guid requesterId, Guid groupId)
    {
        await EnsureRights(decoratee, 
            "GetGroupRoles", 
            requesterId,
            groupId);
        
        return await decoratee.GetGroupRoles(requesterId, groupId);
    }

    public async Task<GroupRoleResponse> GetGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId)
    {
        await EnsureRights(decoratee, 
            "GetGroupRole", 
            requesterId,
            groupId,
            groupRoleId.GetType());
        
        return await decoratee.GetGroupRole(requesterId, groupId, groupRoleId);
    }

    public async Task<GroupRoleResponse> CreateGroupRole(Guid requesterId, Guid groupId, CreateGroupRoleRequest request)
    {
        await EnsureRights(decoratee, 
            "CreateGroupRole", 
            requesterId,
            groupId,
            request.GetType());
        
        return await decoratee.CreateGroupRole(requesterId, groupId, request);
    }

    public async Task<GroupRoleResponse> UpdateGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId, UpdateGroupRoleRequest request)
    {
        await EnsureRights(decoratee, 
            "UpdateGroupRole", 
            requesterId,
            groupId,
            groupRoleId.GetType(),
            request.GetType());
        
        return await decoratee.UpdateGroupRole(requesterId, groupId, groupRoleId, request);
    }

    public async Task DeleteGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId)
    {
        await EnsureRights(decoratee, 
            "DeleteGroupRole", 
            requesterId,
            groupId,
            groupRoleId.GetType());
        
        await decoratee.DeleteGroupRole(requesterId, groupId, groupRoleId);
    }

    public async Task<GroupRoleResponse> AddRightToGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId, AddRightToGroupRoleRequest request)
    {
        await EnsureRights(decoratee, 
            "AddRightToGroupRole", 
            requesterId,
            groupId,
            groupRoleId.GetType(),
            request.GetType());
        
        return await decoratee.AddRightToGroupRole(requesterId, groupId, groupRoleId, request);
    }

    public async Task<GroupRoleResponse> RemoveRightFromGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId, Guid groupRightId)
    {
        await EnsureRights(decoratee, 
            "RemoveRightFromGroupRole", 
            requesterId,
            groupId,
            groupRoleId.GetType(),
            groupRightId.GetType());
        
        return await decoratee.RemoveRightFromGroupRole(requesterId, groupId, groupRoleId, groupRightId);
    }

    public Task<IEnumerable<GroupRoleResponse>> GetDefaultGroupRoles()
    {
        return decoratee.GetDefaultGroupRoles();
    }

    public Task<GroupRoleResponse> CreateDefaultGroupRole(CreateDefaultGroupRoleRequest request)
    {
        return decoratee.CreateDefaultGroupRole(request);
    }

    public Task<GroupRoleResponse> UpdateDefaultGroupRole(Guid defaultGroupRoleId, UpdateGroupRoleRequest request)
    {
        return decoratee.UpdateDefaultGroupRole(defaultGroupRoleId, request);
    }
}