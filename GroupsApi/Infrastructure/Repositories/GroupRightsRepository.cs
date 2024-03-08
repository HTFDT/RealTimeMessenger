using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class GroupRightsRepository(ApplicationDbContext context) 
    : BaseRepository<GroupRight>(context), IGroupRightsRepository
{
    public Task<GroupRight?> GetByGroupRightNameAsync(string name)
    {
        return Entities.SingleOrDefaultAsync(gr => gr.NormalizedName == name.ToUpper());
    }
}