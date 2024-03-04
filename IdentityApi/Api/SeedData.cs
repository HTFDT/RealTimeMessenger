using Dal.Entities;
using Dal.Repository.Interfaces;

namespace Api;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
    {
        var superuserId = await EnsureUser(serviceProvider, testUserPw, "jija");
        await EnsureRole(serviceProvider, superuserId, "Superuser");
    }

    private static async Task<Guid> EnsureUser(IServiceProvider serviceProvider,
        string testUserPw, string username)
    {
        var usersRepository = serviceProvider.GetRequiredService<IUserRepository>();
        var user = await usersRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            user = new UserDal
            {
                Username = username,
                Password = testUserPw
            };
            await usersRepository.CreateAsync(user);
        }

        return user.Id;
    }

    private static async Task EnsureRole(IServiceProvider serviceProvider,
        Guid uid, string role)
    {
        var rolesRepository = serviceProvider.GetRequiredService<IRoleRepository>();
        

        if (await rolesRepository.GetByRoleNameAsync(role) is null)
            await rolesRepository.CreateAsync(new RoleDal
            {
                Name = role
            });
        

        var usersRepository = serviceProvider.GetRequiredService<IUserRepository>();

        var user = await usersRepository.GetByIdAsync(uid);
        if (user is not null)
            await usersRepository.AddUserRoleAsync(user.Id, role);
    }
}