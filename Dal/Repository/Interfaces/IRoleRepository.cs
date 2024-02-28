using Dal.Entities;

namespace Dal.Repository.Interfaces;

public interface IRoleRepository : IGuidRepository<RoleDal>
{
    RoleDal? GetByRoleName(string roleName);
    Task<RoleDal?> GetByRoleNameAsync(string roleName);
    List<UserDal> GetAllUsersInRole(string roleName);
    Task<List<UserDal>> GetAllUsersInRoleAsync(string roleName);
}