using Dal.Entities;
using Dal.Enums;
using Dal.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.EfCore;

internal class EfCoreUserRepository(ApplicationDbContext context, IRoleRepository roleRepository)
    : EfCoreRepositoryBase<UserDal>(context), IUserRepository
{
    public UserDal? GetByUsername(string username)
    {
        return Entities.AsNoTracking().SingleOrDefault(u => u.Username == username);
    }

    public Task<UserDal?> GetByUsernameAsync(string username)
    {
        return Entities.AsNoTracking().SingleOrDefaultAsync(u => u.Username == username);
    }

    public List<string> GetUserRoles(Guid userId)
    {
        var user = GetById(userId);
        if (user is null)
            throw new InvalidOperationException($"No such user with id '{userId}'");
        
        return Entities
            .Where(u => u.Id == userId)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SelectMany(u => u.UserRoles.Select(ur => ur.Role.Name))
            .ToList();
    }

    public async Task<List<string>> GetUserRolesAsync(Guid userId)
    {
        var user = await GetByIdAsync(userId);
        if (user is null)
            throw new InvalidOperationException($"No such user with id '{userId}'");
        return await Entities
            .Where(u => u.Id == userId)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SelectMany(u => u.UserRoles.Select(ur => ur.Role.Name))
            .ToListAsync();
    }

    public void AddUserRole(Guid userId, string roleName)
    {
        var role = roleRepository.GetByRoleName(roleName);
        if (role is null)
            throw new InvalidOperationException($"No such role '{roleName}'");
        var ur = context.UsersRoles
            .SingleOrDefault(ur => ur.UserId == userId && ur.RoleId == role.Id);
        if (ur is not null)
            return;
        context.UsersRoles.Add(new UserRoleDal
        {
            RoleId = role.Id,
            UserId = userId
        });
        context.SaveChanges();
    }

    public async Task AddUserRoleAsync(Guid userId, string roleName)
    {
        var role = await roleRepository.GetByRoleNameAsync(roleName);
        if (role is null)
            throw new InvalidOperationException($"No such role '{roleName}'");
        var ur = await context.UsersRoles
            .SingleOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == role.Id);
        if (ur is not null)
            return;
        context.UsersRoles.Add(new UserRoleDal
        {
            RoleId = role.Id,
            UserId = userId
        });
        await context.SaveChangesAsync();
    }

    public void RemoveUserRole(Guid userId, string roleName)
    {
        var userRole = Entities.Include(e => e.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SelectMany(u => u.UserRoles)
            .SingleOrDefault(ur => ur.Role.Name == roleName);
        if (userRole is null)
            return;
        context.UsersRoles.Remove(userRole);
        context.SaveChanges();
    }

    public async Task RemoveUserRoleAsync(Guid userId, string roleName)
    {
        var userRole = await Entities.Include(e => e.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SelectMany(u => u.UserRoles)
            .SingleOrDefaultAsync(ur => ur.Role.Name == roleName);
        if (userRole is null)
            return;
        context.UsersRoles.Remove(userRole);
        await context.SaveChangesAsync();
    }

    public void SetUserProfile(Guid userId, Gender? gender = null, string? profileDescription = null)
    {
        var user = Entities.FirstOrDefault(u => u.Id == userId);
        if (user is null)
            throw new InvalidOperationException($"No such user with id '{userId}'");
        context.Entry(user).Reference(u => u.Profile).Load();
        user.Profile!.Gender = gender;
        user.Profile!.ProfileDescription = profileDescription;
        context.SaveChanges();
    }

    public async Task SetUserProfileAsync(Guid userId, Gender? gender = null, string? profileDescription = null)
    {
        var user = await Entities.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            throw new InvalidOperationException($"No such user with id '{userId}'");
        await context.Entry(user).Reference(u => u.Profile).LoadAsync();
        user.Profile!.Gender = gender;
        user.Profile!.ProfileDescription = profileDescription;
        await context.SaveChangesAsync();
    }

    public void SetUserRefreshToken(Guid userId, string refreshToken, DateTime expiryDate)
    {
        var user = Entities.FirstOrDefault(u => u.Id == userId);
        if (user is null)
            throw new InvalidOperationException($"No such user with id '{userId}'");
        context.Entry(user).Reference(u => u.RefreshToken).Load();
        if (user.RefreshToken is null)
            user.RefreshToken = new RefreshTokenDal
            {
                RefreshToken = refreshToken,
                ExpiryDate = expiryDate
            };
        else
        {
            user.RefreshToken!.RefreshToken = refreshToken;
            user.RefreshToken!.ExpiryDate = expiryDate;
        }
        context.SaveChanges();
    }

    public async Task SetUserRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryDate)
    {
        var user = await Entities.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            throw new InvalidOperationException($"No such user with id '{userId}'");
        await context.Entry(user).Reference(u => u.RefreshToken).LoadAsync();
        if (user.RefreshToken is null)
            user.RefreshToken = new RefreshTokenDal
            {
                RefreshToken = refreshToken,
                ExpiryDate = expiryDate
            };
        else
        {
            user.RefreshToken!.RefreshToken = refreshToken;
            user.RefreshToken!.ExpiryDate = expiryDate;
        }
        await context.SaveChangesAsync();
    }

    public void RevokeUserRefreshToken(Guid userId)
    {
        var user = Entities.FirstOrDefault(u => u.Id == userId);
        if (user is null)
            throw new InvalidOperationException($"No such user with id '{userId}'");
        context.Entry(user).Reference(u => u.RefreshToken).Load();
        if (user.RefreshToken is null) return;
        context.RefreshTokens.Remove(user.RefreshToken);
        context.SaveChanges();

    }

    public async Task RevokeUserRefreshTokenAsync(Guid userId)
    {
        var user = await Entities.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            throw new InvalidOperationException($"No such user with id '{userId}'");
        await context.Entry(user).Reference(u => u.RefreshToken).LoadAsync();
        if (user.RefreshToken is null)
            return;
        context.RefreshTokens.Remove(user.RefreshToken);
        await context.SaveChangesAsync();
    }

    public RefreshTokenDal? GetUserRefreshToken(Guid userId)
    {
        var user = Entities.FirstOrDefault(u => u.Id == userId);
        if (user is null)
            throw new InvalidOperationException($"No such user with id '{userId}'");
        context.Entry(user).Reference(u => u.RefreshToken).Load();
        return user.RefreshToken;
    }

    public async Task<RefreshTokenDal?> GetUserRefreshTokenAsync(Guid userId)
    {
        var user = await Entities.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            throw new InvalidOperationException($"No such user with id '{userId}'");
        await context.Entry(user).Reference(u => u.RefreshToken).LoadAsync();
        return user.RefreshToken;
    }

    public ProfileDal GetUserProfile(Guid userId)
    {
        var user = Entities.FirstOrDefault(u => u.Id == userId);
        if (user is null)
            throw new InvalidOperationException($"No such user with id '{userId}'");
        context.Entry(user).Reference(u => u.Profile).Load();
        return user.Profile!;
    }

    public async Task<ProfileDal> GetUserProfileAsync(Guid userId)
    {
        var user = await Entities.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            throw new InvalidOperationException($"No such user with id '{userId}'");
        await context.Entry(user).Reference(u => u.Profile).LoadAsync();
        return user.Profile!;
    }

    public bool UserHasRole(Guid userId, string roleName)
    {
        return Entities.Where(u => u.Id == userId)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SelectMany(u => u.UserRoles.Where(ur => ur.Role.Name == roleName))
            .Any();
    }

    public Task<bool> UserHasRoleAsync(Guid userId, string roleName)
    {
        return Entities.Where(u => u.Id == userId)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SelectMany(u => u.UserRoles.Where(ur => ur.Role.Name == roleName))
            .AnyAsync();
    }
}