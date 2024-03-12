using Domain.Interfaces;
using Services.Interfaces.Interfaces;
using Shared.Dto.Groups;
using Shared.Dto.Memberships;

namespace Services.Decorators;

internal class MembershipsServiceEnsureRightsDecorator(IMembershipsService decoratee, IRepositoryManager repoManager)
    : BaseEnsureRightsDecorator(repoManager), IMembershipsService
{
    public Task<MembershipResponse> JoinGroup(Guid requesterId, Guid groupId)
    {
        return decoratee.JoinGroup(requesterId, groupId);
    }

    public Task<MembershipResponse> AddMemberToPublicGroup(Guid requesterId, Guid groupId, AddMemberRequest request)
    {
        return decoratee.AddMemberToPublicGroup(requesterId, groupId, request);
    }

    public async Task<MembershipResponse> AddMemberToPrivateGroup(Guid requesterId, Guid groupId, AddMemberRequest request)
    {
        await EnsureRights(decoratee, 
            "AddMemberToPrivateGroup", 
            requesterId,
            groupId,
            request.GetType());
        
        return await decoratee.AddMemberToPrivateGroup(requesterId, groupId, request);
    }

    public async Task<MembershipResponse> KickMember(Guid requesterId, Guid groupId, Guid kickedId)
    {
        await EnsureRights(decoratee, 
            "KickMember", 
            requesterId,
            groupId,
            kickedId.GetType());
        
        return await decoratee.KickMember(requesterId, groupId, kickedId);
    }

    public async Task<IEnumerable<MembershipResponse>> GetAllMembers(Guid requesterId, Guid groupId)
    {
        await EnsureRights(decoratee, 
            "GetAllMembers", 
            requesterId,
            groupId);
        
        return await decoratee.GetAllMembers(requesterId, groupId);
    }

    public async Task<MembershipResponse> GetMember(Guid requesterId, Guid groupId, Guid memberId)
    {
        await EnsureRights(decoratee, 
            "GetMember", 
            requesterId,
            groupId,
            memberId.GetType());
        
        return await decoratee.GetMember(requesterId, groupId, memberId);
    }

    public Task<MembershipResponse> LeaveGroup(Guid requesterId, Guid groupId)
    {
        return decoratee.LeaveGroup(requesterId, groupId);
    }

    public async Task<MembershipResponse> AssignRoleToMember(Guid requesterId, Guid groupId, Guid memberId, AssignRoleToMemberRequest request)
    {
        await EnsureRights(decoratee, 
            "AssignRoleToMember", 
            requesterId,
            groupId,
            memberId.GetType(),
            request.GetType());
        
        return await decoratee.AssignRoleToMember(requesterId, groupId, memberId, request);
    }
}