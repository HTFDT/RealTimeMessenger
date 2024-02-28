using Dal.Entities;
using Dal.Repository;
using Dal.Repository.Interfaces;
using Logic.Models;
using Logic.Models.Profile;
using Logic.Models.User;

namespace Logic;

public class UserManager(IUserRepository userRepository)
{
    public async Task<Guid> CreateUser(UserInModel userIn)
    {
        var entity = new UserDal
        {
            Username = userIn.Username,
            Password = userIn.Password
        };

        await userRepository.CreateAsync(entity);
        return entity.Id;
    }

    public async Task<bool> CheckPasswordAsync(string username, string password)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user is null)
            throw new InvalidOperationException($"User with username '{username}' doesn't exist");
        return user.Password == password;
    }

    public async Task<UserOutModel?> GetById(Guid id)
    {
        var entity = await userRepository.GetByIdAsync(id);
        if (entity is null)
            return null;
        return new UserOutModel
        {
            Id = entity.Id,
            Username = entity.Username
        };
    }
    
    public async Task<UserOutModel?> GetByUsername(string username)
    {
        var entity = await userRepository.GetByUsernameAsync(username);
        if (entity is null)
            return null;
        return new UserOutModel
        {
            Id = entity.Id,
            Username = entity.Username
        };
    }
    
    public async Task<bool> UpdateUserPassword(Guid id, string currentPassword, string newPassword)
    {
        var entity = await userRepository.GetByIdAsync(id);
        if (entity is null)
            throw new InvalidOperationException($"User with id '{id}' doesn't exist");

        if (currentPassword != entity.Password) 
            return false;
        entity.Password = newPassword;
        await userRepository.UpdateAsync(entity);
        return true;
    }

    public Task DeleteUser(Guid id)
    {
        return userRepository.DeleteByIdAsync(id);
    }
    
    public async Task<bool> SetUserProfile(Guid userId, ProfileModel profile)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
            throw new InvalidOperationException($"User with id '{userId}' doesn't exist");
        await userRepository.SetUserProfileAsync(userId, profile.Gender, profile.ProfileDescription);
        return true;
    }
}