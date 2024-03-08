using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class GroupRolesRepository(ApplicationDbContext context) : BaseRepository<GroupRole>(context), IGroupRolesRepository
{
    public Task<List<GroupRole>> GetAllRolesInGroup(Guid groupId)
    {
        return Entities.Where(e => e.GroupId == groupId).ToListAsync();
    }

    public Task<GroupRole?> GetByGroupIdAndRoleId(Guid groupId, Guid roleId)
    {
        return Entities.SingleOrDefaultAsync(e => e.GroupId == groupId && e.Id == roleId);
    }

    public Task<GroupRole?> GetDefaultMemberRole()
    {
        return Entities.SingleOrDefaultAsync(e => e.IsDefaultMemberRole);
    }

    public Task<GroupRole?> GetDefaultKickedRole()
    {
        return Entities.SingleOrDefaultAsync(e => e.IsDefaultKickedRole);
    }

    public Task<GroupRole?> GetDefaultOwnerRole()
    {
        return Entities.SingleOrDefaultAsync(e => e.IsDefaultOwnerRole);
    }

    public Task<GroupRole?> GetDefaultLeftRole()
    {
        return Entities.SingleOrDefaultAsync(e => e.IsDefaultLeftRole);
    }
}