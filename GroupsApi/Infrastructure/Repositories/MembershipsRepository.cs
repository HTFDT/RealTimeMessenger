using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class MembershipsRepository(ApplicationDbContext context) : BaseRepository<Membership>(context), IMembershipsRepository
{
    public Task<Membership?> GetByUserIdAndGroupId(Guid userId, Guid groupId)
    {
        return Entities.SingleOrDefaultAsync(e => e.UserId == userId && e.GroupId == groupId);
    }

    public Task<Membership?> GetByMemberIdAndGroupId(Guid groupId, Guid memberId)
    {
        return Entities.SingleOrDefaultAsync(e => e.Id == memberId && e.GroupId == groupId);
    }

    public Task<List<Guid>> GetUsersInGroup(Guid groupId, bool includeKicked)
    {
        return Entities.Where(e => e.GroupId == groupId && (includeKicked || !e.IsKicked))
            .Select(e => e.UserId)
            .ToListAsync();
    }
}