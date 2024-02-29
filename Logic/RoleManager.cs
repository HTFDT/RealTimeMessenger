using Dal.Entities;
using Dal.Repository.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Logic;

public class RoleManager(IRoleRepository roleRepository)
{
    public async Task<Guid> CreateRole(RoleInModel roleIn)
    {
        var entity = new RoleDal
        {
            Name = roleIn.RoleName,
        };
        return await roleRepository.CreateAsync(entity);
    }

    public async Task DeleteRole(Guid id)
    {
        await roleRepository.DeleteByIdAsync(id);
    }

    public async Task<IEnumerable<RoleOutModel>> GetAllRoles()
    {
        var roles = await roleRepository.GetAllAsync();
        return roles.Select(r => new RoleOutModel
        {
            Id = r.Id,
            RoleName = r.Name!
        }).ToList();
    }
}