using Shared.Dto.Groups;
using Shared.Dto.Memberships;

namespace Services.Interfaces.Interfaces;

public interface IMembershipsServiceWrapper
{
    Task<MembershipResponse> JoinGroup(Guid requesterId, Guid groupId);
    Task<MembershipResponse> AddMember(Guid requesterId, Guid groupId, AddMemberRequest request);
    Task<MembershipResponse> KickMember(Guid requesterId, Guid groupId, Guid kickedId);
    Task<IEnumerable<MembershipResponse>> GetAllMembers(Guid requesterId, Guid groupId);
    Task<MembershipResponse> GetMember(Guid requesterId, Guid groupId, Guid memberId);
    Task<MembershipResponse> LeaveGroup(Guid requesterId, Guid groupId);
    Task<MembershipResponse> AssignRoleToMember(Guid requesterId, Guid groupId, Guid memberId, AssignRoleToMemberRequest request);
}