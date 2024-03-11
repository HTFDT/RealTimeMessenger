using Domain.Exceptions;
using Domain.Interfaces;
using Services.Interfaces.Interfaces;
using Shared.Dto.Groups;
using Shared.Dto.Memberships;

namespace Services.Implementations;

internal class MembershipsServiceWrapper(IMembershipsService membershipsService, IRepositoryManager repoManager) : IMembershipsServiceWrapper
{
    public Task<MembershipResponse> JoinGroup(Guid requesterId, Guid groupId)
    {
        return membershipsService.JoinGroup(requesterId, groupId);
    }

    public async Task<MembershipResponse> AddMember(Guid requesterId, Guid groupId, AddMemberRequest request)
    {
        var group = await repoManager.Groups.GetByIdAsync(groupId);
        if (group is null)
            throw new GroupNotFoundException(groupId);
        if (group.IsPrivate)
            return await membershipsService.AddMemberToPrivateGroup(requesterId, groupId, request);
        return await membershipsService.AddMemberToPublicGroup(requesterId, groupId, request);
    }

    public Task<MembershipResponse> KickMember(Guid requesterId, Guid groupId, Guid kickedId)
    {
        return membershipsService.KickMember(requesterId, groupId, kickedId);
    }

    public Task<IEnumerable<MembershipResponse>> GetAllMembers(Guid requesterId, Guid groupId)
    {
        return membershipsService.GetAllMembers(requesterId, groupId);
    }

    public Task<MembershipResponse> GetMember(Guid requesterId, Guid groupId, Guid memberId)
    {
        return membershipsService.GetMember(requesterId, groupId, memberId);
    }

    public Task<MembershipResponse> LeaveGroup(Guid requesterId, Guid groupId)
    {
        return membershipsService.LeaveGroup(requesterId, groupId);
    }

    public Task<MembershipResponse> AssignRoleToMember(Guid requesterId, Guid groupId, Guid memberId, AssignRoleToMemberRequest request)
    {
        return membershipsService.AssignRoleToMember(requesterId, groupId, memberId, request);
    }
}