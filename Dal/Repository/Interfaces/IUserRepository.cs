using Core.Base.Dal;
using Dal.Entities;
using Dal.Enums;

namespace Dal.Repository.Interfaces;

public interface IUserRepository : IGuidRepository<UserDal>
{
    UserDal? GetByUsername(string username);
    Task<UserDal?> GetByUsernameAsync(string username);
    List<string> GetUserRoles(Guid userId);
    Task<List<string>> GetUserRolesAsync(Guid userId);
    void AddUserRole(Guid userId, string roleName);
    Task AddUserRoleAsync(Guid userId, string roleName);
    void RemoveUserRole(Guid userId, string roleName);
    Task RemoveUserRoleAsync(Guid userId, string roleName);
    void SetUserProfile(Guid userId, Gender? gender=null, string? profileDescription=null);
    Task SetUserProfileAsync(Guid userId, Gender? gender=null, string? profileDescription=null);
    void SetUserRefreshToken(Guid userId, string refreshToken, DateTime expiryDate);
    Task SetUserRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryDate);
    void RevokeUserRefreshToken(Guid userId);
    Task RevokeUserRefreshTokenAsync(Guid userId);
    RefreshTokenDal? GetUserRefreshToken(Guid userId);
    Task<RefreshTokenDal?> GetUserRefreshTokenAsync(Guid userId);
    ProfileDal GetUserProfile(Guid userId);
    Task<ProfileDal> GetUserProfileAsync(Guid userId);
    bool UserHasRole(Guid userId, string roleName);
    Task<bool> UserHasRoleAsync(Guid userId, string roleName);
}