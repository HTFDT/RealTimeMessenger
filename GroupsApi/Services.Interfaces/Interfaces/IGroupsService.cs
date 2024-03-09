using Shared.Dto.Groups;
using Shared.Dto.Memberships;

namespace Services.Interfaces.Interfaces;

public interface IGroupsService
{
    Task<IEnumerable<GroupResponse>> GetAllGroups();
    Task<GroupResponse> GetGroup(Guid groupId);
    Task<GroupResponse> CreateGroup(Guid requesterId, CreateGroupRequest request);
    Task<GroupResponse> UpdateGroup(Guid requestedId, Guid groupId, UpdateGroupRequest request);
    Task DeleteGroup(Guid requesterId, Guid groupId);
    Task AddTag(Guid requesterId, Guid groupId, AddTagRequest request);
    Task RemoveTag(Guid requesterId, Guid groupId, Guid tagId);
    Task<GroupResponse> SetDefaultMemberRole(Guid requesterId, Guid groupId, SetDefaultMemberRoleRequest request);
}