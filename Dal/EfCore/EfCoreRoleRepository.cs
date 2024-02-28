using Dal.Entities;
using Dal.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.EfCore;

internal class EfCoreRoleRepository(ApplicationDbContext context)
    : EfCoreRepositoryBase<RoleDal>(context), IRoleRepository
{
    public override Guid Create(RoleDal entity)
    {
        entity.NormalizedName = entity.Name.ToUpper();
        Entities.Add(entity);
        context.SaveChanges();
        return entity.Id;
    }
    
    public override async Task<Guid> CreateAsync(RoleDal entity)
    {
        entity.NormalizedName = entity.Name.ToUpper();
        Entities.Add(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public RoleDal? GetByRoleName(string roleName)
    {
        return Entities.AsNoTracking()
            .SingleOrDefault(r => r.NormalizedName == roleName.ToUpper());
    }

    public Task<RoleDal?> GetByRoleNameAsync(string roleName)
    {
        return Entities.AsNoTracking()
            .SingleOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper());
    }

    public List<UserDal> GetAllUsersInRole(string roleName)
    {
        var role = GetByRoleName(roleName);
        if (role is null)
            throw new InvalidOperationException($"No such role '{roleName}'");
        return Entities
            .AsNoTracking()
            .Where(r => r.Id == role.Id)
            .Include(r => r.UserRoles)
            .ThenInclude(ur => ur.User)
            .SelectMany(r => r.UserRoles.Select(ur => ur.User))
            .ToList();
    }

    public async Task<List<UserDal>> GetAllUsersInRoleAsync(string roleName)
    {
        var role = await GetByRoleNameAsync(roleName);
        if (role is null)
            throw new InvalidOperationException($"No such role '{roleName}'");
        return await Entities
            .AsNoTracking()
            .Where(r => r.Id == role.Id)
            .Include(r => r.UserRoles)
            .ThenInclude(ur => ur.User)
            .SelectMany(r => r.UserRoles.Select(ur => ur.User))
            .ToListAsync();
    }
}