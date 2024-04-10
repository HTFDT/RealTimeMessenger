using IdentityConnectionLib.ConnectionServices.Dtos;
using Logic;

namespace Api.Consumers;

/// <summary>
/// обработчик для запроса UserInfoIdentityApiRequest
/// </summary>
public class UserInfoConsumer(IServiceProvider serviceProvider) : BaseIdentityConsumer<UserInfoIdentityApiRequest, UserInfoIdentityApiResponse>
{
    protected override async Task<UserInfoIdentityApiResponse?> HandleMessage(UserInfoIdentityApiRequest request)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager>();
        var user = await userManager.GetById(request.UserId);
        return user is null ? null : new UserInfoIdentityApiResponse(user.Id, user.Username);
    }
}