using Domain.Entities;

namespace Domain.Interfaces;

public interface IMembershipsRepository : IRepository<Membership>
{
    Task<Membership?> GetByUserIdAndGroupId(Guid userId, Guid groupId);
    Task<Membership?> GetByMemberIdAndGroupId(Guid groupId, Guid memberId);
    Task<List<Guid>> GetUsersInGroup(Guid groupId, bool includeKicked);
}