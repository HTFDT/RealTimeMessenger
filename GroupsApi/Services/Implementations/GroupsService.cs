using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Services.Attributes;
using Services.Interfaces.Interfaces;
using Shared.Dto.Groups;

namespace Services.Implementations;

internal class GroupsService(IRepositoryManager repositoryManager) : IGroupsService
{
    public async Task<IEnumerable<GroupResponse>> GetAllGroups()
    {
        var groups = await repositoryManager.Groups.GetAllAsync();
        foreach (var gr in groups)
            await repositoryManager.Groups.LoadCollection(gr, g => g.Tags);
        return groups.Select(g => new GroupResponse(g.Id,
            g.Name,
            g.Description,
            g.IsPrivate,
            g.DefaultMemberRoleId,
            g.Tags.Select(t => t.Id)));
    }

    public async Task<GroupResponse> GetGroup(Guid groupId)
    {
        var group = await CheckGroupExists(groupId);
        await repositoryManager.Groups.LoadCollection(group, g => g.Tags);
        return new GroupResponse(group.Id,
            group.Name,
            group.Description,
            group.IsPrivate,
            group.DefaultMemberRoleId,
            group.Tags.Select(t => t.Id));
    }

    public async Task<GroupResponse> CreateGroup(Guid requesterId, CreateGroupRequest request)
    {
        // проверяем, существует ли дефолтная роль участника группы
        var defaultMemberRole = await repositoryManager.GroupRoles.GetDefaultMemberRole();
        if (defaultMemberRole is null)
            throw new DefaultMemberRoleNotFoundException();
        // проверяем, существуют ли запрашиваемые теги
        var group = new Group
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreationDate = DateTime.UtcNow,
            DefaultMemberRoleId = defaultMemberRole.Id,
            IsPrivate = request.IsPrivate,
            Tags = new List<Tag>()
        };
        var distinctIds = request.GroupTagIds.Distinct().ToList();
        foreach (var tagId in distinctIds)
            group.Tags.Add(await CheckTagExists(tagId));
        
        // проверяем, существует ли дефолтная роль владельца группы
        var defaultOwnerRole = await repositoryManager.GroupRoles.GetDefaultOwnerRole();
        if (defaultOwnerRole is null)
            throw new DefaultOwnerRoleNotFoundException();
        
        await repositoryManager.Groups.CreateAsync(group);
        // создаём сущность пользователя как члена группы в роли создателя
        await repositoryManager.Memberships.CreateAsync(new Membership
        {
            UserId = requesterId,
            GroupId = group.Id,
            GroupRoleId = defaultOwnerRole.Id,
            IsRoleUnique = defaultOwnerRole.IsUnique,
            DateJoined = DateTime.UtcNow,
            LastMessageNumberWhenJoined = group.LastMessageNum,
            IsKicked = false,
            IsLeft = false,
            IsOwner = true
        });
        await repositoryManager.UnitOfWork.SaveChangesAsync();
        return new GroupResponse(group.Id,
            group.Name,
            group.Description, 
            group.IsPrivate, 
            group.DefaultMemberRoleId,
            distinctIds);
    }
    
    [EnsureRequesterRights("EditGroup")]
    public async Task<GroupResponse> UpdateGroup(Guid requestedId, Guid groupId, UpdateGroupRequest request)
    {
        var group = await CheckGroupExists(groupId);
        await repositoryManager.Groups.LoadCollection(group, g => g.Tags);
        group.Name = request.Name;
        group.Description = request.Description;
        group.IsPrivate = request.IsPrivate;
        var allIds = group.Tags.Select(t => t.Id).ToList();
        // составляем сэт уже прикреплённых тэгов
        var existingIds = request.GroupTagIds.Intersect(allIds).ToHashSet();
        // оставляем только тэги, которые не нужно удалять
        group.Tags = group.Tags.Where(t => existingIds.Contains(t.Id)).ToList();
        // составляем список тэгов, которые нужно добавить
        var idsToAdd = request.GroupTagIds.Except(allIds).ToList();
        // добавляем тэги
        foreach (var tagId in idsToAdd)
            group.Tags.Add(await CheckTagExists(tagId));
        
        await repositoryManager.Groups.UpdateAsync(group);
        await repositoryManager.UnitOfWork.SaveChangesAsync();
        return new GroupResponse(group.Id,
            group.Name,
            group.Description, 
            group.IsPrivate, 
            group.DefaultMemberRoleId,
            existingIds.Concat(idsToAdd));
    }
    
    [EnsureRequesterRights("DeleteGroup")]
    public async Task DeleteGroup(Guid requesterId, Guid groupId)
    {
        var group = await CheckGroupExists(groupId);
        await repositoryManager.Groups.DeleteAsync(group);
        await repositoryManager.UnitOfWork.SaveChangesAsync();
    }

    [EnsureRequesterRights("EditGroup")]
    public async Task AddTag(Guid requesterId, Guid groupId, AddTagRequest request)
    {
        var group = await CheckGroupExists(groupId);
        var tag = await CheckTagExists(request.TagId);
        await repositoryManager.Groups.LoadCollection(group, g => g.Tags);
        if (group.Tags.Select(t => t.Id).Contains(request.TagId))
            throw new GroupTagConflictException(groupId, request.TagId);
        group.Tags.Add(tag);
        await repositoryManager.Groups.UpdateAsync(group);
        await repositoryManager.UnitOfWork.SaveChangesAsync();
    }

    [EnsureRequesterRights("EditGroup")]
    public async Task RemoveTag(Guid requesterId, Guid groupId, Guid tagId)
    {
        var group = await CheckGroupExists(groupId);
        await repositoryManager.Groups.LoadCollection(group, g => g.Tags);
        group.Tags = group.Tags.Where(t => t.Id != tagId).ToList();
        await repositoryManager.Groups.UpdateAsync(group);
        await repositoryManager.UnitOfWork.SaveChangesAsync();
    }

    [EnsureRequesterRights("EditGroup")]
    public async Task<GroupResponse> SetDefaultMemberRole(Guid requesterId, Guid groupId, SetDefaultMemberRoleRequest request)
    {
        var group = await CheckGroupExists(groupId);
        await repositoryManager.Groups.LoadCollection(group, g => g.GroupRoles);
        // ищем подходящую роль среди ролей группы
        var role = group.GroupRoles.SingleOrDefault(gr => gr.Id == request.GroupRoleId);
        
        if (role is null)
        {
            // если нет такой роли группы, ищем среди дефолтных
            role = (await repositoryManager.GroupRoles
                .FilterAsync(gr => gr.Id == request.GroupRoleId && gr.IsDefault)).SingleOrDefault();
            // если всё еще нет такой, то кидаем исключение
            if (role is null)
                throw new GroupRoleNotFoundException(groupId, request.GroupRoleId);
        }
        // если роль уникальна среди членов группы, кидаем с исключение, так как такие роли не могут быть дефолтными
        if (role.IsUnique)
            throw new DefaultUniqueRoleForbiddenException(role.Id);
        
        // если роль является дефолтной ролью для кикнутых пользователей, то кидаем исключение
        if (role.IsDefaultKickedRole)
            throw new DefaultKickedRoleAsDefaultMemberRoleForbiddenException(role.Id);
            
        group.DefaultMemberRoleId = request.GroupRoleId;
        await repositoryManager.Groups.UpdateAsync(group);
        await repositoryManager.UnitOfWork.SaveChangesAsync();
        await repositoryManager.Groups.LoadCollection(group, g => g.Tags);
        return new GroupResponse(group.Id,
            group.Name,
            group.Description, 
            group.IsPrivate, 
            group.DefaultMemberRoleId,
            group.Tags.Select(t => t.Id));
    }

    private async Task<Group> CheckGroupExists(Guid groupId)
    {
        var group = await repositoryManager.Groups.GetByIdAsync(groupId);
        if (group is null)
            throw new GroupNotFoundException(groupId);
        return group;
    }
    
    private async Task<Tag> CheckTagExists(Guid tagId)
    {
        var tag = await repositoryManager.Tags.GetByIdAsync(tagId);
        if (tag is null)
            throw new GroupNotFoundException(tagId);
        return tag;
    }
}