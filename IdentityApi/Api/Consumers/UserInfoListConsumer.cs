using IdentityConnectionLib.ConnectionServices.Dtos;
using Logic;

namespace Api.Consumers;


/// <summary>
/// обработчик для запроса UserInfoListIdentityApiRequest
/// </summary>
public class UserInfoListConsumer(IServiceProvider serviceProvider) : BaseIdentityConsumer<UserInfoListIdentityApiRequest,
    IEnumerable<UserInfoListIdentityApiResponse>>
{
    protected override async Task<IEnumerable<UserInfoListIdentityApiResponse>?> HandleMessage(UserInfoListIdentityApiRequest request)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager>();
        var users = await userManager.GetAllUsers();
        return users.Select(u => new UserInfoListIdentityApiResponse(u.Id, u.Username));
    }
}