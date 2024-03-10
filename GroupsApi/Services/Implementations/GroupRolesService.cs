using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Services.Interfaces.Interfaces;
using Shared.Dto.GroupRights;
using Shared.Dto.GroupRoles;

namespace Services.Implementations;

internal class GroupRolesService(IRepositoryManager repoManager) : IGroupRolesService
{
    public async Task<IEnumerable<GroupRoleResponse>> GetGroupRoles(Guid requesterId, Guid groupId)
    {
        var group = await CheckGroupExists(groupId);
        await repoManager.Groups.LoadCollection(group, g => g.GroupRoles);
        foreach (var gr in group.GroupRoles)
            await repoManager.GroupRoles.LoadCollection(gr, gRole => gRole.GroupRights);
        return group.GroupRoles.Select(gr => new GroupRoleResponse(gr.Id,
            gr.GroupId,
            gr.IsUnique,
            gr.IsDefault,
            gr.IsRevocable,
            gr.Name,
            gr.Description,
            gr.GroupRights.Select(gRight => gRight.Id)));
    }

    public async Task<GroupRoleResponse> GetGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId)
    {
        var group = await CheckGroupExists(groupId);
        var gr = await CheckGroupRoleExists(groupId, groupRoleId);
        await repoManager.GroupRoles.LoadCollection(gr, g => g.GroupRights);
        return new GroupRoleResponse(gr.Id,
            gr.GroupId,
            gr.IsUnique,
            gr.IsDefault,
            gr.IsRevocable,
            gr.Name,
            gr.Description,
            gr.GroupRights.Select(gRight => gRight.Id));
    }

    public async Task<GroupRoleResponse> CreateGroupRole(Guid requesterId, Guid groupId, CreateGroupRoleRequest request)
    {
        var group = await CheckGroupExists(groupId);
        var newRole = new GroupRole
        {
            IsRevocable = true,
            IsAssignableByUsers = true,
            IsUnique = request.IsUnique,
            GroupId = group.Id,
            Name = request.Name,
            Description = request.Description,
            GroupRights = new List<GroupRight>()
        };
        var distinctIds = request.GroupRights.Distinct().ToList();
        foreach (var grId in distinctIds)
            newRole.GroupRights.Add(await CheckGroupRightExists(grId));

        await repoManager.GroupRoles.CreateAsync(newRole);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new GroupRoleResponse(newRole.Id,
            newRole.GroupId,
            newRole.IsUnique,
            newRole.IsDefault,
            newRole.IsRevocable,
            newRole.Name,
            newRole.Description,
            distinctIds);
    }

    public async Task<GroupRoleResponse> UpdateGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId,
        UpdateGroupRoleRequest request)
    {
        var group = await CheckGroupExists(groupId);
        var groupRole = await CheckGroupRoleExists(groupId, groupRoleId);
        groupRole.Name = request.Name;
        groupRole.Description = request.Description;
        var idsToReturn = await UpdateGroupRoleRights(groupRole, request.GroupRights);
        
        await repoManager.GroupRoles.UpdateAsync(groupRole);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new GroupRoleResponse(groupRole.Id,
            groupRole.GroupId,
            groupRole.IsUnique,
            groupRole.IsDefault,
            groupRole.IsRevocable,
            groupRole.Name,
            groupRole.Description,
            idsToReturn);
    }

    public async Task DeleteGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId)
    {
        var group = await CheckGroupExists(groupId);
        var groupRole = await CheckGroupRoleExists(group.Id, groupRoleId);
        if (group.DefaultMemberRoleId == groupRole.Id)
        {
            var defaultMemberRole = await repoManager.GroupRoles.GetDefaultMemberRole();
            if (defaultMemberRole is null)
                throw new DefaultMemberRoleNotFoundException();
            group.DefaultMemberRoleId = defaultMemberRole.Id;
            await repoManager.Groups.UpdateAsync(group);
        }
        await repoManager.GroupRoles.LoadCollection(groupRole, gr => gr.Memberships);
        foreach (var member in groupRole.Memberships)
            member.GroupRoleId = group.DefaultMemberRoleId;
        
        await repoManager.GroupRoles.DeleteAsync(groupRole);
        await repoManager.UnitOfWork.SaveChangesAsync();
    }

    public async Task<GroupRoleResponse> AddRightToGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId,
        AddRightToGroupRoleRequest request)
    {
        var group = await CheckGroupExists(groupId);
        var groupRole = await CheckGroupRoleExists(groupId, groupRoleId);
        var groupRight = await CheckGroupRightExists(request.GroupRightId);
        await repoManager.GroupRoles.LoadCollection(groupRole, gr => gr.GroupRights);
        var groupRightIds = groupRole.GroupRights.Select(gr => gr.Id).ToList();
        if (groupRightIds.Contains(request.GroupRightId))
            throw new GroupRoleGroupRightConflictException(groupId, groupRoleId);
        groupRole.GroupRights.Add(groupRight);
        await repoManager.GroupRoles.UpdateAsync(groupRole);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new GroupRoleResponse(groupRole.Id,
            groupRole.GroupId,
            groupRole.IsUnique,
            groupRole.IsDefault,
            groupRole.IsRevocable,
            groupRole.Name,
            groupRole.Description,
            groupRightIds);
    }

    public async Task<GroupRoleResponse> RemoveRightFromGroupRole(Guid requesterId, Guid groupId, Guid groupRoleId,
        Guid groupRightId)
    {
        var group = await CheckGroupExists(groupId);
        var groupRole = await CheckGroupRoleExists(groupId, groupRoleId);
        await repoManager.GroupRoles.LoadCollection(groupRole, g => g.GroupRights);
        groupRole.GroupRights = groupRole.GroupRights.Where(gr => gr.Id != groupRightId).ToList();
        await repoManager.GroupRoles.UpdateAsync(groupRole);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new GroupRoleResponse(groupRole.Id,
            groupRole.GroupId,
            groupRole.IsUnique,
            groupRole.IsDefault,
            groupRole.IsRevocable,
            groupRole.Name,
            groupRole.Description,
            groupRole.GroupRights.Select(gr => gr.Id));
    }

    public async Task<IEnumerable<GroupRoleResponse>> GetDefaultGroupRoles()
    {
        var defaultRoles = await repoManager.GroupRoles.FilterAsync(gr => gr.IsDefault);
        foreach (var dr in defaultRoles)
            await repoManager.GroupRoles.LoadCollection(dr, r => r.GroupRights);
        return defaultRoles.Select(dr => new GroupRoleResponse(dr.Id,
            dr.GroupId,
            dr.IsUnique,
            dr.IsDefault,
            dr.IsRevocable,
            dr.Name,
            dr.Description,
            dr.GroupRights.Select(gr => gr.Id)));
    }

    public async Task<GroupRoleResponse> CreateDefaultGroupRole(CreateDefaultGroupRoleRequest request)
    {
        var role = new GroupRole
        {
            IsRevocable = request.IsRevocable,
            IsUnique = request.IsUnique,
            IsAssignableByUsers = request.IsAssignableByUsers,
            IsDefault = true,
            IsDefaultMemberRole = request.IsDefaultMemberRole,
            IsDefaultKickedRole = request.IsDefaultKickedRole,
            IsDefaultLeftRole = request.IsDefaultLeftRole,
            IsDefaultOwnerRole = request.IsDefaultOwnerRole,
            Name = request.Name,
            Description = request.Description,
            GroupId = null,
            GroupRights = new List<GroupRight>()
        };
        var distinctIds = request.GroupRights.Distinct().ToList();
        foreach (var grId in distinctIds)
            role.GroupRights.Add(await CheckGroupRightExists(grId));
        await repoManager.GroupRoles.CreateAsync(role);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new GroupRoleResponse(role.Id,
            role.GroupId,
            role.IsUnique,
            role.IsDefault,
            role.IsRevocable,
            role.Name,
            role.Description,
            role.GroupRights.Select(gr => gr.Id));
    }

    public async Task<GroupRoleResponse> UpdateDefaultGroupRole(Guid defaultGroupRoleId, UpdateGroupRoleRequest request)
    {
        var defaultRole = (await repoManager.GroupRoles
            .FilterAsync(r => r.IsDefault && r.Id == defaultGroupRoleId)).SingleOrDefault();
        if (defaultRole is null)
            throw new DefaultRoleNotFoundException(defaultGroupRoleId);
        defaultRole.Name = request.Name;
        defaultRole.Description = request.Description;
        var idsToReturn = await UpdateGroupRoleRights(defaultRole, request.GroupRights);
        
        await repoManager.GroupRoles.UpdateAsync(defaultRole);
        await repoManager.UnitOfWork.SaveChangesAsync();
        return new GroupRoleResponse(defaultRole.Id,
            defaultRole.GroupId,
            defaultRole.IsUnique,
            defaultRole.IsDefault,
            defaultRole.IsRevocable,
            defaultRole.Name,
            defaultRole.Description,
            idsToReturn);
    }

    private async Task<Group> CheckGroupExists(Guid groupId)
    {
        var group = await repoManager.Groups.GetByIdAsync(groupId);
        if (group is null)
            throw new GroupNotFoundException(groupId);
        return group;
    }

    private async Task<GroupRole> CheckGroupRoleExists(Guid groupId, Guid groupRoleId)
    {
        var gr = await repoManager.GroupRoles.GetByGroupIdAndRoleId(groupId, groupRoleId);
        if (gr is null)
            throw new GroupRoleNotFoundException(groupId, groupRoleId);
        return gr;
    }
    
    private async Task<GroupRight> CheckGroupRightExists(Guid groupRightId)
    {
        var gr = await repoManager.GroupRights.GetByIdAsync(groupRightId);
        if (gr is null)
            throw new GroupRightNotFoundException(groupRightId);
        return gr;
    }

    private async Task<IEnumerable<Guid>> UpdateGroupRoleRights(GroupRole groupRole, IEnumerable<Guid> newRightsIds)
    {
        await repoManager.GroupRoles.LoadCollection(groupRole, gr => gr.GroupRights);
        var allIds = groupRole.GroupRights.Select(t => t.Id).ToList();
        newRightsIds = newRightsIds.ToList();
        // составляем сэт уже прикреплённых прав
        var existingIds = newRightsIds.Intersect(allIds).ToHashSet();
        // оставляем только права, которые не нужно удалять
        groupRole.GroupRights = groupRole.GroupRights.Where(gr => existingIds.Contains(gr.Id)).ToList();
        // составляем список тэгов, которые нужно добавить
        var idsToAdd = newRightsIds.Except(allIds).ToList();
        // добавляем права
        foreach (var grId in idsToAdd)
            groupRole.GroupRights.Add(await CheckGroupRightExists(grId));
        return existingIds.Concat(idsToAdd);
    }
}