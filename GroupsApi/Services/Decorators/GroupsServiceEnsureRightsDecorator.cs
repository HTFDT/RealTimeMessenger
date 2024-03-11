using Domain.Interfaces;
using Services.Interfaces.Interfaces;
using Shared.Dto.Groups;

namespace Services.Decorators;

internal class GroupsServiceEnsureRightsDecorator(IGroupsService decoratee, IRepositoryManager repoManager) 
    : BaseEnsureRightsDecorator(repoManager), IGroupsService
{
    public Task<IEnumerable<GroupResponse>> GetAllGroups()
    {
        return decoratee.GetAllGroups();
    }

    public Task<GroupResponse> GetGroup(Guid groupId)
    {
        return decoratee.GetGroup(groupId);
    }

    public Task<GroupResponse> CreateGroup(Guid requesterId, CreateGroupRequest request)
    {
        return decoratee.CreateGroup(requesterId, request);
    }

    public async Task<GroupResponse> UpdateGroup(Guid requestedId, Guid groupId, UpdateGroupRequest request)
    {
        await EnsureRights(decoratee, 
            "UpdateGroup", 
            requestedId,
            groupId,
            request.GetType());
        
        return await decoratee.UpdateGroup(requestedId, groupId, request);
    }

    public async Task DeleteGroup(Guid requesterId, Guid groupId)
    {
        await EnsureRights(decoratee, 
            "DeleteGroup", 
            requesterId,
            groupId);
        
        await decoratee.DeleteGroup(requesterId, groupId);
    }

    public async Task AddTag(Guid requesterId, Guid groupId, AddTagRequest request)
    {
        await EnsureRights(decoratee, 
            "AddTag", 
            requesterId,
            groupId,
            request.GetType());
        
        await decoratee.AddTag(requesterId, groupId, request);
    }

    public async Task RemoveTag(Guid requesterId, Guid groupId, Guid tagId)
    {
        await EnsureRights(decoratee, 
            "RemoveTag", 
            requesterId,
            groupId,
            tagId.GetType());
        
        await decoratee.RemoveTag(requesterId, groupId, tagId);
    }

    public async Task<GroupResponse> SetDefaultMemberRole(Guid requesterId, Guid groupId, SetDefaultMemberRoleRequest request)
    {
        await EnsureRights(decoratee, 
            "SetDefaultMemberRole", 
            requesterId,
            groupId,
            request.GetType());
        
        return await decoratee.SetDefaultMemberRole(requesterId, groupId, request);
    }
}